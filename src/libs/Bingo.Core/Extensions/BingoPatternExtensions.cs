using Bingo.Core.Patterns;

namespace Bingo.Core.Extensions;

public static class BingoPatternExtensions
{
	public static bool UsesColumn(this BingoPattern pattern, int col) =>
		pattern.Cells.Any(c => c.Col == col);

	public static bool Matches(this BingoPattern pattern, IReadOnlySet<(int Row, int Col)> markedCells)
	{
		return pattern.GetActiveCells().All(cell => markedCells.Contains((cell.Row, cell.Col)));
	}
}
