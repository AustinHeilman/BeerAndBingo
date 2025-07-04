using System.Diagnostics;
using System.Text.Json;

namespace Bingo.Core.Patterns;

public abstract class PatternRepositoryBase
{
	protected readonly Dictionary<string, BingoPatternFile> PatternIndex = new(StringComparer.OrdinalIgnoreCase);

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

	protected abstract string GetSaveDirectory();

	public virtual async Task InitializeAsync()
	{
		PatternIndex.Clear();

		if (!Directory.Exists(GetSaveDirectory()))
			Directory.CreateDirectory(GetSaveDirectory());

		var files = Directory.EnumerateFiles(GetSaveDirectory(), "*-pattern.json");
		foreach (var file in files)
		{
			try
			{
				var json = await File.ReadAllTextAsync(file);
				var pattern = JsonSerializer.Deserialize<BingoPattern>(json);
				if (pattern is not null)
				{
					var fileInfo = new FileInfo(file);
					PatternIndex[pattern.Name] = BingoPatternFile.From(pattern, fileInfo);
				}
			}
			catch
			{
				Debug.WriteLine($"Skipped malformed pattern file: {file}");
			}
		}
	}
	public virtual bool HasPatternNamed(string name) => PatternIndex.ContainsKey(name);
}
