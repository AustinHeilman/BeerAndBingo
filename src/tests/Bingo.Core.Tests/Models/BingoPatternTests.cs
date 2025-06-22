using Bingo.Core.Models;
using Bingo.Core.Extensions;
using Xunit;

namespace Bingo.Core.Tests.Models;

public class BingoPatternTests
{
    [Fact]
    public void GetUsedColumns_ReturnsCorrectColumns()
    {
        var pattern = new BingoPattern
        {
            Cells = new()
            {
                (0, 0), (1, 2), (2, 14)
            }
        };

        var used = pattern.GetUsedColumns();

        Assert.Contains(0, used);
        Assert.Contains(2, used);
        Assert.Contains(14, used);
        Assert.Equal(3, used.Count);
    }

    [Fact]
    public void Rotate90_RotatesPatternCorrectly()
    {
        var pattern = new BingoPattern
        {
            Cells = new()
            {
                (0, 0), // top-left
                (4, 14) // bottom-right
            }
        };

        var rotated = pattern.Rotate90();

        Assert.Contains((0, 4), rotated); // originally (0,0)
        Assert.Contains((14, 0), rotated); // originally (4,14)
        Assert.Equal(2, rotated.Count);
    }

    [Fact]
    public void Matches_ReturnsTrueForCompleteMatch()
    {
        var pattern = new BingoPattern
        {
            Cells = new()
            {
                (1, 1), (2, 2)
            }
        };

        var playerMarks = new HashSet<(int, int)> { (1, 1), (2, 2), (3, 3) };

        Assert.True(pattern.Matches(playerMarks));
    }

    [Fact]
    public void Matches_ReturnsFalseIfAnyCellMissing()
    {
        var pattern = new BingoPattern
        {
            Cells = new()
            {
                (1, 1), (2, 2)
            }
        };

        var playerMarks = new HashSet<(int, int)> { (1, 1) };

        Assert.False(pattern.Matches(playerMarks));
    }
}
