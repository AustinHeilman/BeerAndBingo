using System.IO;
using System.Text.Json;

namespace Bingo.Core.Patterns;

public class BingoPatternFile : BingoPattern
{
	public required FileInfo File { get; set; }

	public DateTime LastModifiedUtc => File.LastWriteTimeUtc;

	public async Task SaveAsync()
	{
		string json = JsonSerializer.Serialize(this as BingoPattern, new JsonSerializerOptions { WriteIndented = true });
		await System.IO.File.WriteAllTextAsync(File.FullName, json);
	}

	public void Delete()
	{
		if (File.Exists)
		{
			File.Delete();
		}
	}

	public BingoPattern ToBingoPattern() =>
		new()
		{
			Name = this.Name,
			Cells = this.Cells.ToHashSet()
		};

	public static BingoPatternFile From(BingoPattern pattern, FileInfo file) =>
		new()
		{
			Name = pattern.Name,
			Cells = pattern.Cells.ToHashSet(),
			File = file
		};
}
