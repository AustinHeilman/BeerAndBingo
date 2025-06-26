using Bingo.Core.Patterns;
using Bingo.Services.Patterns;

namespace Bingo.AppServices.Patterns;

public class WinningPatternEvaluator : IWinningPatternEvaluator
{
	public bool IsWinning(BingoPattern pattern, IReadOnlySet<(int row, int col)> markedCells)
	{
		return pattern.Cells.All(cell => markedCells.Contains(cell));
	}
}
