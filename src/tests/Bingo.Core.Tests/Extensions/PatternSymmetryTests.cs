using Bingo.Core.Extensions;
using Bingo.Core.Patterns;

namespace Bingo.Core.Tests.Extensions;

public class PatternSymmetryTests
{
	[Fact]
	public void IsSymmetrical_ReturnsTrueForMirror()
	{
		BingoPattern symmetrical = new()
		{
			Cells = new()
			{
				(0, 2), (0, 12),
				(1, 5), (1, 9),
				(2, 7) // center column, mirrors itself
            }
		};

		Assert.True(symmetrical.IsSymmetrical());
	}

	[Fact]
	public void IsSymmetrical_ReturnsFalseForAsymmetry()
	{
		BingoPattern asymmetrical = new()
		{
			Cells = new()
			{
				(0, 1), (0, 13) // should be symmetrical...
                // ...but let's remove one side
            }
		};

		asymmetrical.Cells.Remove((0, 13));

		Assert.False(asymmetrical.IsSymmetrical());
	}
}
