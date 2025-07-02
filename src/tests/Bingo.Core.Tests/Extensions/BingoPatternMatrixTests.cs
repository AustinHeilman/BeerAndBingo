using Bingo.Core.Extensions;
using Bingo.Core.Patterns;

namespace Bingo.Core.Tests.Extensions
{
	public class BingoPatternMatrixTests
	{
		[Fact]
		public void ToMatrix_Creates_Expected_Grid()
		{
			BingoPattern pattern = new()
			{
				Cells = new HashSet<PatternCell>
				{
					new() { Row = 0, Col = 0, IsActive = true },
					new() { Row = 1, Col = 1, IsActive = true },
					new() { Row = 2, Col = 2, IsActive = true }
				}
			};

			bool[,] matrix = pattern.ToMatrix();

			Assert.True(matrix[0, 0]);
			Assert.True(matrix[1, 1]);
			Assert.True(matrix[2, 2]);
			Assert.False(matrix[0, 1]);
		}

		[Fact]
		public void FromMatrix_Creates_Expected_Cells()
		{
			bool[,] input =
			{
				{ true,  false },
				{ false, true }
			};

			BingoPattern pattern = BingoPatternMatrixExtensions.FromMatrix(input);

			// 🟣 New checks — full 2×2 grid
			Assert.Equal(4, pattern.Cells.Count);
			Assert.Equal(2, pattern.GetActiveCells().Count());

			Assert.Contains(pattern.Cells, c => c.Row == 0 && c.Col == 0 && c.IsActive);
			Assert.Contains(pattern.Cells, c => c.Row == 1 && c.Col == 1 && c.IsActive);
			Assert.Contains(pattern.Cells, c => c.Row == 0 && c.Col == 1 && !c.IsActive);
			Assert.Contains(pattern.Cells, c => c.Row == 1 && c.Col == 0 && !c.IsActive);
		}
	}
}
