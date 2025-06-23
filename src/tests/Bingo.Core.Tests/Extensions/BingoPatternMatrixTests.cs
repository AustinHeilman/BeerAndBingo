using Bingo.Core.Extensions;
using Bingo.Core.Models;

namespace Bingo.Core.Tests.Extensions;

public class BingoPatternMatrixTests
{
    [Fact]
    public void ToMatrix_CorrectlyMapsCells()
    {
        var pattern = new BingoPattern
        {
            Cells = new()
            {
                (0, 0), (2, 5), (4, 14)
            }
        };

        var matrix = pattern.ToMatrix();

        Assert.True(matrix[0, 0]);
        Assert.True(matrix[2, 5]);
        Assert.True(matrix[4, 14]);
        Assert.False(matrix[1, 1]);
    }

    [Fact]
    public void FromMatrix_ReconstructsSamePattern()
    {
        var original = new BingoPattern
        {
            Cells = new()
            {
                (1, 1), (2, 2)
            }
        };

        var matrix = original.ToMatrix();
        var reconstructed = BingoPatternMatrixExtensions.FromMatrix(matrix);

        Assert.Equal(original.Cells, reconstructed.Cells);
    }
}
