using Bingo.Core.Models;
using Bingo.AppServices.Patterns;
using Xunit;

namespace Bingo.AppServices.Tests.Patterns;

public class WinningPatternEvaluatorTests
{
    private readonly WinningPatternEvaluator _evaluator = new();

    [Fact]
    public void IsWinning_ReturnsTrue_WhenAllPatternCellsAreMarked()
    {
        var pattern = new BingoPattern
        {
            Name = "Line",
            Cells = new HashSet<(int, int)> { (0, 0), (0, 1), (0, 2) }
        };

        var marked = new HashSet<(int, int)> { (0, 0), (0, 1), (0, 2), (1, 3) };

        Assert.True(_evaluator.IsWinning(pattern, marked));
    }

    [Fact]
    public void IsWinning_ReturnsFalse_WhenSomeCellsAreMissing()
    {
        var pattern = new BingoPattern
        {
            Name = "L",
            Cells = new HashSet<(int, int)> { (0, 0), (1, 0), (2, 0) }
        };

        var marked = new HashSet<(int, int)> { (1, 0), (2, 0) };

        Assert.False(_evaluator.IsWinning(pattern, marked));
    }
}
