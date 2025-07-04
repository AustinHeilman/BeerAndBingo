using System.Diagnostics;
using System.Text.Json;

namespace Bingo.Core.Patterns;

public abstract class PatternRepositoryBase
{
	protected readonly Dictionary<string, BingoPatternFile> PatternIndex = new(StringComparer.OrdinalIgnoreCase);
	protected virtual string FileSearchPattern => "*-pattern.json";
	public virtual Task<IEnumerable<BingoPattern>> LoadAllFromFilesAsync()
	{
		return Task.FromResult(PatternIndex.Values.Cast<BingoPattern>());
	}

	public virtual Task<BingoPattern?> GetByNameAsync(string name)
	{
		PatternIndex.TryGetValue(name, out var pattern);
		return Task.FromResult(pattern as BingoPattern);
	}

	public virtual async Task SaveAsync(BingoPattern pattern)
	{
		if (PatternIndex.TryGetValue(pattern.Name, out var existing))
		{
			existing.Cells = pattern.Cells.ToHashSet();
			await existing.SaveAsync();
		}
		else
		{
			var newPath = Path.Combine(GetSaveDirectory(), GenerateFileName());
			var file = new FileInfo(newPath);
			var patternFile = BingoPatternFile.From(pattern, file);
			await patternFile.SaveAsync();
			PatternIndex[pattern.Name] = patternFile;
		}
	}

	public virtual Task DeleteAsync(string name)
	{
		if (PatternIndex.TryGetValue(name, out var file))
		{
			file.Delete();
			PatternIndex.Remove(name);
		}
		return Task.CompletedTask;
	}

	protected virtual string GenerateFileName() =>
		$"{Guid.NewGuid():N}-pattern.json";

	public abstract string GetSaveDirectory();

	private bool _isInitialized;
	private readonly SemaphoreSlim _initLock = new(1, 1);

	public virtual async Task InitializeAsync()
	{
		await InitializeAsync(false);
	}

	public virtual async Task InitializeAsync(bool forceReload = false)
	{
		if (!Directory.Exists(GetSaveDirectory()))
			Directory.CreateDirectory(GetSaveDirectory()); //Create it before trying to read

		var files = Directory.EnumerateFiles(GetSaveDirectory(), FileSearchPattern, SearchOption.TopDirectoryOnly);
		foreach (var file in files)
		{
			try
			{
				var json = await File.ReadAllTextAsync(file);
				var dto = JsonSerializer.Deserialize<PatternJsonModel>(json);
				if (dto is not null)
				{
					var pattern = dto.ToDomain(); // your extension method
					var fileInfo = new FileInfo(file);
					PatternIndex[pattern.Name] = WrapPattern(pattern, fileInfo);
					Debug.WriteLine($"[Vault] Loaded pattern: {pattern.Name}");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Skipped malformed pattern file '{file}': {ex.Message}");
			}
		}
	}
	public virtual bool HasPatternNamed(string name) => PatternIndex.ContainsKey(name);

	protected virtual BingoPatternFile WrapPattern(BingoPattern pattern, FileInfo file) =>
	BingoPatternFile.From(pattern, file);
}
