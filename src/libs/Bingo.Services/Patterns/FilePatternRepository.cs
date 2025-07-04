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

	public override string GetSaveDirectory() => _directory;

	protected override string GenerateFileName() =>
		$"{Guid.NewGuid():N}{FileSuffix}";

	protected override string FileSearchPattern => $"*{FileSuffix}";

	protected override BingoPatternFile WrapPattern(BingoPattern pattern, FileInfo file) =>
		BingoPatternFile.From(pattern, file);
}
