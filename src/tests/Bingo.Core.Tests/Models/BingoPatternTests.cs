using Bingo.Core.Extensions;
using Bingo.Core.Models;

namespace Bingo.Core.Tests.Models;

public class BingoPatternTests
{
    [Fact]
    public void GetUsedColumns_ReturnsCorrectColumns()
    {
        BingoPattern pattern = new()
        {
            Cells = new()
            {
                (0, 0), (1, 2), (2, 14)
            }
        };

        HashSet<int> used = pattern.GetUsedColumns();

        Assert.Contains(0, used);
        Assert.Contains(2, used);
        Assert.Contains(14, used);
        Assert.Equal(3, used.Count);
    }

    [Fact]
    public void Rotate90_RotatesPatternCorrectly()
    {
        BingoPattern pattern = new()
        {
            Cells = new()
            {
                (0, 0), // top-left
                (4, 14) // bottom-right
            }
        };

        HashSet<(int Row, int Col)> rotated = pattern.Rotate90();

        Assert.Contains((0, 4), rotated); // originally (0,0)
        Assert.Contains((14, 0), rotated); // originally (4,14)
        Assert.Equal(2, rotated.Count);
    }

    [Fact]
    public void Matches_ReturnsTrueForCompleteMatch()
    {
        BingoPattern pattern = new()
        {
            Cells = new()
            {
                (1, 1), (2, 2)
            }
        };

        HashSet<(int, int)> playerMarks = new()
        { (1, 1), (2, 2), (3, 3) };

        Assert.True(pattern.Matches(playerMarks));
    }

    [Fact]
    public void Matches_ReturnsFalseIfAnyCellMissing()
    {
        BingoPattern pattern = new()
        {
            Cells = new()
            {
                (1, 1), (2, 2)
            }
        };

        HashSet<(int, int)> playerMarks = new()
        { (1, 1) };

        Assert.False(pattern.Matches(playerMarks));
    }
}
