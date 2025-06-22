using Bingo.Core.Models;

namespace Bingo.Core.Extensions;

public static class BingoPatternExtensions
{
    public static bool UsesColumn(this BingoPattern pattern, int col) =>
        pattern.Cells.Any(c => c.Col == col);

    public static HashSet<(int Row, int Col)> Rotate90(this BingoPattern pattern, int totalRows = 5, int totalCols = 15)
    {
        return pattern.Cells
            .Select(cell => (Row: cell.Col, Col: totalRows - 1 - cell.Row))
            .ToHashSet();
    }

    public static bool Matches(this BingoPattern pattern, IReadOnlySet<(int Row, int Col)> markedCells)
    {
        return pattern.Cells.All(markedCells.Contains);
    }
}
