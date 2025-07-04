using System.Diagnostics;
using System.Text.Json;
using Bingo.Core.Patterns;

namespace Bingo.Services.Patterns;

public class FilePatternRepository : PatternRepositoryBase
{
	private readonly string _directory;
	private const string FileSuffix = "-pattern.json";

	public FilePatternRepository(string rootDirectory)
	{
		_directory = Path.Combine(rootDirectory, "Patterns");
	}

	protected override string GetSaveDirectory() => _directory;

	protected override string GenerateFileName() =>
		$"{Guid.NewGuid():N}{FileSuffix}";

	public override async Task InitializeAsync()
	{
		PatternIndex.Clear();

		if (!Directory.Exists(_directory))
			Directory.CreateDirectory(_directory);

		var files = Directory.EnumerateFiles(_directory, $"*{FileSuffix}");
		foreach (var file in files)
		{
			try
			{
				string json = await File.ReadAllTextAsync(file);
				var pattern = JsonSerializer.Deserialize<BingoPattern>(json);
				if (pattern is not null)
				{
					var fileInfo = new FileInfo(file);
					var fileWrapper = BingoPatternFile.From(pattern, fileInfo);
					PatternIndex[pattern.Name] = fileWrapper;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Failed to load pattern file '{file}': {ex.Message}");
			}
		}
	}

	public override async Task SaveAsync(BingoPattern pattern)
	{
		if (PatternIndex.TryGetValue(pattern.Name, out var existing))
		{
			existing.Cells = pattern.Cells.ToHashSet();
			await existing.SaveAsync();
		}
		else
		{
			var file = new FileInfo(Path.Combine(_directory, GenerateFileName()));
			var patternFile = BingoPatternFile.From(pattern, file);
			await patternFile.SaveAsync();
			PatternIndex[pattern.Name] = patternFile;
		}
	}

	public override Task DeleteAsync(string name)
	{
		if (PatternIndex.TryGetValue(name, out var patternFile))
		{
			patternFile.Delete();
			PatternIndex.Remove(name);
		}

		return Task.CompletedTask;
	}
}
