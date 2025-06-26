using Bingo.Core.Extensions;
using Bingo.Core.Patterns;

namespace Bingo.Core.Tests.Extensions;

public class BingoPatternMatrixTests
{
	[Fact]
	public void ToMatrix_CorrectlyMapsCells()
	{
		BingoPattern pattern = new()
		{
			Cells = new()
			{
				(0, 0), (2, 5), (4, 14)
			}
		};

		bool[,] matrix = pattern.ToMatrix();

		Assert.True(matrix[0, 0]);
		Assert.True(matrix[2, 5]);
		Assert.True(matrix[4, 14]);
		Assert.False(matrix[1, 1]);
	}

	[Fact]
	public void FromMatrix_ReconstructsSamePattern()
	{
		BingoPattern original = new()
		{
			Cells = new()
			{
				(1, 1), (2, 2)
			}
		};

		bool[,] matrix = original.ToMatrix();
		BingoPattern reconstructed = BingoPatternMatrixExtensions.FromMatrix(matrix);

		Assert.Equal(original.Cells, reconstructed.Cells);
	}
}
