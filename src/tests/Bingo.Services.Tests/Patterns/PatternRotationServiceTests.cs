using Bingo.Services.Patterns;

namespace Bingo.Services.Tests.Patterns;

public class PatternRotationServiceTests
{
	[Fact]
	public void Rotate90_RotatesCoordinatesClockwise()
	{
		HashSet<(int row, int col)> original = new()
		{
			(0, 0), // top-left
            (4, 14) // bottom-right
        };

		ISet<(int row, int col)> rotated = PatternRotationService.Rotate90(original);

		Assert.Contains((0, 4), rotated);   // (0,0) ➜ (0,4)
		Assert.Contains((14, 0), rotated);  // (4,14) ➜ (14,0)
		Assert.Equal(2, rotated.Count);
	}

	[Fact]
	public void Rotate180_IsEquivalentToTwo90s()
	{
		HashSet<(int, int)> original = new()
		{
			(1, 2), (3, 5)
		};

		ISet<(int row, int col)> once = PatternRotationService.Rotate90(original);
		ISet<(int row, int col)> twice = PatternRotationService.Rotate90(once);

		ISet<(int row, int col)> rotated180 = PatternRotationService.Rotate180(original);

		Assert.Equal(rotated180, twice);
	}

	[Fact]
	public void Rotate270_IsEquivalentToThree90s()
	{
		HashSet<(int, int)> original = new()
		{
			(2, 1), (4, 0)
		};

		ISet<(int row, int col)> once = PatternRotationService.Rotate90(original);
		ISet<(int row, int col)> twice = PatternRotationService.Rotate90(once);
		ISet<(int row, int col)> thrice = PatternRotationService.Rotate90(twice);

		ISet<(int row, int col)> rotated270 = PatternRotationService.Rotate270(original);

		Assert.Equal(rotated270, thrice);
	}

	[Fact]
	public void RotatingTwiceReturnsToOriginal_With180And180()
	{
		HashSet<(int, int)> original = new()
		{
			(0, 3), (4, 11)
		};

		ISet<(int row, int col)> rotated180 = PatternRotationService.Rotate180(original);
		ISet<(int row, int col)> rotatedBack = PatternRotationService.Rotate180(rotated180);

		Assert.Equal(original, rotatedBack);
	}
}
