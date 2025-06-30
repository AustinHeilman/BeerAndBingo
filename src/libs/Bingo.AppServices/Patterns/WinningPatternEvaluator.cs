using Bingo.Core.Games.Bingo.Patterns;

namespace Bingo.AppServices.Patterns;

public static class WinningPatternEvaluator
{
	public static bool IsWinning(this BingoPattern pattern, HashSet<(int, int)> marked)
	{
		return pattern.GetActiveCells()
			.All(c => marked.Contains((c.Row, c.Col)));
	}
}
