using Bingo.Core.Patterns;

namespace Bingo.Core.Extensions;

public static class BingoPatternExtensions
{
	public static bool UsesColumn(this BingoPattern pattern, int col) =>
		pattern.Cells.Any(c => c.Col == col);

	public static HashSet<(int Row, int Col)> Rotate90(
		this BingoPattern pattern,
		int totalRows,
		int totalCols)
	{
		return pattern.Cells
			.Select(cell => (Row: cell.Col, Col: totalRows - 1 - cell.Row))
			.ToHashSet();
	}

	public static HashSet<(int Row, int Col)> Rotate90(
		this BingoPattern pattern)
	{
		return Rotate90(pattern, PatternGridSettings.PatternRowCount, PatternGridSettings.PatternColCount);
	}

	public static bool Matches(this BingoPattern pattern, IReadOnlySet<(int Row, int Col)> markedCells)
	{
		return pattern.Cells.All(markedCells.Contains);
	}
}
