using System.Text.Json;

namespace Bingo.Core.Patterns;

public class BingoPatternFile : BingoPattern
{
	public required FileInfo File { get; set; }

	public DateTime LastModifiedUtc => File.LastWriteTimeUtc;

	public async Task SaveAsync()
	{
		var dto = this.ToJsonModel(); // extension method you created earlier
		string json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
		{
			WriteIndented = true
		});

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
