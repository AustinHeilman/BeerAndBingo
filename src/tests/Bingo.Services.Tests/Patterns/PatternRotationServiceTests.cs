using Bingo.Services.Patterns;
using Xunit;

namespace Bingo.Services.Tests.Patterns;

public class PatternRotationServiceTests
{
    [Fact]
    public void Rotate90_RotatesCoordinatesClockwise()
    {
        var original = new HashSet<(int row, int col)>
        {
            (0, 0), // top-left
            (4, 14) // bottom-right
        };

        var rotated = PatternRotationService.Rotate90(original, 5, 15);

        Assert.Contains((0, 4), rotated);   // (0,0) ➜ (0,4)
        Assert.Contains((14, 0), rotated);  // (4,14) ➜ (14,0)
        Assert.Equal(2, rotated.Count);
    }

    [Fact]
    public void Rotate180_IsEquivalentToTwo90s()
    {
        var original = new HashSet<(int, int)>
        {
            (1, 2), (3, 5)
        };

        var once = PatternRotationService.Rotate90(original, 5, 15);
        var twice = PatternRotationService.Rotate90(once, 15, 5);

        var rotated180 = PatternRotationService.Rotate180(original, 5, 15);

        Assert.Equal(rotated180, twice);
    }

    [Fact]
    public void Rotate270_IsEquivalentToThree90s()
    {
        var original = new HashSet<(int, int)>
        {
            (2, 1), (4, 0)
        };

        var once = PatternRotationService.Rotate90(original, 5, 15);
        var twice = PatternRotationService.Rotate90(once, 15, 5);
        var thrice = PatternRotationService.Rotate90(twice, 5, 15);

        var rotated270 = PatternRotationService.Rotate270(original, 5, 15);

        Assert.Equal(rotated270, thrice);
    }

    [Fact]
    public void RotatingTwiceReturnsToOriginal_With180And180()
    {
        var original = new HashSet<(int, int)>
        {
            (0, 3), (4, 11)
        };

        var rotated180 = PatternRotationService.Rotate180(original, 5, 15);
        var rotatedBack = PatternRotationService.Rotate180(rotated180, 5, 15);

        Assert.Equal(original, rotatedBack);
    }
}
