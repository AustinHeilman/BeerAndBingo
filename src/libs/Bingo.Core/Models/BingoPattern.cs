namespace Bingo.Core.Models;

public class BingoPattern
{
    public string Name { get; set; } = string.Empty;

    // Which cells are part of this pattern (row: 0-4, column: 0-14)
    public HashSet<(int Row, int Col)> Cells { get; set; } = new();

    // Optional: Used to determine which bingo columns are needed for calling
    public HashSet<int> GetUsedColumns() =>
        Cells.Select(c => c.Col).ToHashSet();
}
