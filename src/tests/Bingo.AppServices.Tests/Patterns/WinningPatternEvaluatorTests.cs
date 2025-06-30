using Bingo.AppServices.Patterns;
using Bingo.Core.Games.Bingo.Patterns;

namespace Bingo.AppServices.Tests.Patterns
{
	public class WinningPatternEvaluatorTests
	{
		[Fact]
		public void Pattern_Matches_Marked_Cells()
		{
			BingoPattern pattern = new()
			{
				Cells = new HashSet<PatternCell>
				{
					new() { Row = 0, Col = 0, IsActive = true },
					new() { Row = 1, Col = 1, IsActive = true }
				}
			};

			HashSet<(int, int)> marked = new()
			{ (0, 0), (1, 1) };

			// Static method call — no object needed
			bool result = WinningPatternEvaluator.IsWinning(pattern, marked);

			Assert.True(result);
		}
	}
}
