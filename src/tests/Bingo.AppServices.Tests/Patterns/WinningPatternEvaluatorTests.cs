using Xunit;
using System.Collections.Generic;
using Bingo.Core.Patterns;
using Bingo.AppServices.Patterns;

namespace Bingo.AppServices.Tests.Patterns
{
	public class WinningPatternEvaluatorTests
	{
		[Fact]
		public void Pattern_Matches_Marked_Cells()
		{
			var pattern = new BingoPattern
			{
				Cells = new HashSet<PatternCell>
				{
					new() { Row = 0, Col = 0, IsActive = true },
					new() { Row = 1, Col = 1, IsActive = true }
				}
			};

			var marked = new HashSet<(int, int)> { (0, 0), (1, 1) };

			// Static method call — no object needed
			bool result = WinningPatternEvaluator.IsWinning(pattern, marked);

			Assert.True(result);
		}
	}
}
