using Bingo.Services.Patterns.Events;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
namespace Bingo.Core.Patterns;

public abstract class PatternRepositoryBase
{
	protected readonly Dictionary<string, BingoPatternFile> PatternIndex = new(StringComparer.OrdinalIgnoreCase);
	protected virtual string FileSearchPattern => "*-pattern.json";
	public virtual Task<IEnumerable<BingoPattern>> GetAllPatternsAsync()
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

		StringBuilder dbg = new StringBuilder();
		dbg.AppendLine($"[Vault] Initializing pattern repository at {GetSaveDirectory()}");
		dbg.Append($"[Vault] Loaded Patterns:");
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
					dbg.Append(pattern.Name + ", ");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Skipped malformed pattern file '{file}': {ex.Message}");
			}
		}
		Debug.WriteLine(dbg.ToString());
	}
	public virtual bool HasPatternNamed(string name) => PatternIndex.ContainsKey(name);

	protected virtual BingoPatternFile WrapPattern(BingoPattern pattern, FileInfo file) =>
	BingoPatternFile.From(pattern, file);

	public async Task AddOrUpdatePattern(BingoPattern pattern)
	{
		if (string.IsNullOrWhiteSpace(pattern.Name) || pattern.Cells == null || pattern.Cells.Count == 0)
		{
			Debug.WriteLine("[Vault] Skipped AddOrUpdate: invalid pattern");
			return;
		}

		if (PatternIndex.TryGetValue(pattern.Name, out var existing))
		{
			Debug.WriteLine($"[Vault] Updating pattern: {pattern.Name}");
			existing.Cells = pattern.Cells;
			await existing.SaveAsync();
		}
		else
		{
			var file = new FileInfo(Path.Combine(GetSaveDirectory(), $"{Guid.NewGuid()}-pattern.json"));
			var newFile = BingoPatternFile.From(pattern, file);
			PatternIndex[pattern.Name] = newFile;
			Debug.WriteLine($"[Vault] Adding new pattern: {pattern.Name}");
			await newFile.SaveAsync();
		}
	}

	public async Task AddOrUpdatePattern(string name, IEnumerable<PatternCell> cells)
	{
		var pattern = new BingoPattern
		{
			Name = name,
			Cells = new HashSet<PatternCell>(cells)
		};

		await AddOrUpdatePattern(pattern);
	}

	public virtual bool RemovePattern(string patternName)
	{
		if (!PatternIndex.TryGetValue(patternName, out var patternFile))
		{
			Debug.WriteLine($"[Vault] Remove skipped: '{patternName}' not found in index.");
			return false;
		}

		try
		{
			patternFile.Delete(); // Will only delete if file exists
			PatternIndex.Remove(patternName);

			Debug.WriteLine($"[Vault] Removed pattern: {patternName}");
			return true;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"[Vault] Failed to delete pattern '{patternName}': {ex.Message}");
			return false;
		}
	}


	#region Active Pattern Management
	public BingoPattern? ActivePattern { get; protected set; }

	public virtual void SetActivePattern(BingoPattern? pattern)
	{
		ActivePattern = pattern ?? BingoPattern.EmptyPattern;
		WeakReferenceMessenger.Default.Send(new PatternChangedEvent(ActivePattern));
	}
	#endregion
}
