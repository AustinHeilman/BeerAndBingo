using Bingo.AppServices.Patterns;
using Bingo.Core.Models;

namespace Bingo.AppServices.Tests.Patterns;

public class WinningPatternEvaluatorTests
{
    private readonly WinningPatternEvaluator _evaluator = new();

    [Fact]
    public void IsWinning_ReturnsTrue_WhenAllPatternCellsAreMarked()
    {
        BingoPattern pattern = new()
        {
            Name = "Line",
            Cells = new HashSet<(int, int)> { (0, 0), (0, 1), (0, 2) }
        };

        HashSet<(int, int)> marked = new()
        { (0, 0), (0, 1), (0, 2), (1, 3) };

        Assert.True(_evaluator.IsWinning(pattern, marked));
    }

    [Fact]
    public void IsWinning_ReturnsFalse_WhenSomeCellsAreMissing()
    {
        BingoPattern pattern = new()
        {
            Name = "L",
            Cells = new HashSet<(int, int)> { (0, 0), (1, 0), (2, 0) }
        };

        HashSet<(int, int)> marked = new()
        { (1, 0), (2, 0) };

        Assert.False(_evaluator.IsWinning(pattern, marked));
    }
}
