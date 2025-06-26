namespace Bingo.Core.Patterns;

public class BingoPattern
{
	public string Name { get; set; } = string.Empty;

	// Which cells are part of this pattern (row: 0-4, column: 0-14)
	public HashSet<(int Row, int Col)> Cells { get; set; } = new();

	// Optional: Used to determine which bingo columns are needed for calling
	public HashSet<int> GetUsedColumns() =>
		Cells.Select(c => c.Col).ToHashSet();

	public static BingoPattern EmptyPattern => new()
	{
		Name = "None",
		Cells = new HashSet<(int Row, int Col)>
		{
			// Create the 5x5 grid: all cells (row 0-4, col 0-4)
			(0, 0), (0, 1), (0, 2), (0, 3), (0, 4),
			(1, 0), (1, 1), (1, 2), (1, 3), (1, 4),
			(2, 0), (2, 1), (2, 2), (2, 3), (2, 4),
			(3, 0), (3, 1), (3, 2), (3, 3), (3, 4),
			(4, 0), (4, 1), (4, 2), (4, 3), (4, 4)
		}
	};
}
