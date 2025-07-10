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
		Debug.WriteLine($"[Repository] Returning {PatternIndex.Count} pattern(s)");
		return Task.FromResult(PatternIndex.Values.Cast<BingoPattern>());
	}

	public virtual Task<BingoPattern?> GetByNameAsync(string name)
	{
		PatternIndex.TryGetValue(name, out BingoPatternFile? pattern);
		return Task.FromResult(pattern as BingoPattern);
	}

	public virtual async Task SaveAsync(BingoPattern pattern)
	{
		if (PatternIndex.TryGetValue(pattern.Name, out BingoPatternFile? existing))
		{
			existing.Cells = pattern.Cells.ToHashSet();
			await existing.SaveAsync();
		}
		else
		{
			string newPath = Path.Combine(GetSaveDirectory(), GenerateFileName());
			FileInfo file = new(newPath);
			BingoPatternFile patternFile = BingoPatternFile.From(pattern, file);
			await patternFile.SaveAsync();
			PatternIndex[pattern.Name] = patternFile;
		}
	}

	public virtual Task DeleteAsync(string name)
	{
		if (PatternIndex.TryGetValue(name, out BingoPatternFile? file))
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

		StringBuilder dbg = new();
		dbg.AppendLine($"[Vault] Initializing pattern repository at {GetSaveDirectory()}");
		dbg.Append($"[Vault] Loaded Patterns:");
		IEnumerable<string> files = Directory.EnumerateFiles(GetSaveDirectory(), FileSearchPattern, SearchOption.TopDirectoryOnly);
		foreach (string file in files)
		{
			try
			{
				string json = await File.ReadAllTextAsync(file);
				PatternJsonModel? dto = JsonSerializer.Deserialize<PatternJsonModel>(json);
				if (dto is not null)
				{
					BingoPattern pattern = dto.ToDomain(); // your extension method
					FileInfo fileInfo = new(file);
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

		if (PatternIndex.TryGetValue(pattern.Name, out BingoPatternFile? existing))
		{
			Debug.WriteLine($"[Vault] Updating pattern: {pattern.Name}");
			existing.Cells = pattern.Cells;
			await existing.SaveAsync();
		}
		else
		{
			FileInfo file = new(Path.Combine(GetSaveDirectory(), $"{Guid.NewGuid()}-pattern.json"));
			BingoPatternFile newFile = BingoPatternFile.From(pattern, file);
			PatternIndex[pattern.Name] = newFile;
			Debug.WriteLine($"[Vault] Adding new pattern: {pattern.Name}");
			await newFile.SaveAsync();
		}
	}

	public async Task AddOrUpdatePattern(string name, IEnumerable<PatternCell> cells)
	{
		BingoPattern pattern = new()
		{
			Name = name,
			Cells = new HashSet<PatternCell>(cells)
		};

		await AddOrUpdatePattern(pattern);
	}

	public virtual bool RemovePattern(string patternName)
	{
		if (!PatternIndex.TryGetValue(patternName, out BingoPatternFile? patternFile))
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
