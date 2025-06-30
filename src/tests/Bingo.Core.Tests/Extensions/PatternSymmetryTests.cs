using Bingo.Core.Games.Bingo.Patterns;
using Bingo.Core.Games.Bingo.Patterns.Extensions;
using static Bingo.Core.Games.Bingo.Patterns.Extensions.IsSymmetricalExtension;

namespace Bingo.Core.Tests.Extensions
{
	public class PatternSymmetryTests
	{
		[Fact]
		public void Diagonal_Symmetry_Is_Detected()
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

			Assert.True(pattern.IsSymmetrical(SymmetryKind.Diagonal));
		}

		[Fact]
		public void Horizontal_Symmetry_Is_Detected()
		{
			BingoPattern pattern = new()
			{
				Cells = new HashSet<PatternCell>
				{
					new() { Row = 0, Col = 0, IsActive = true },
					new() { Row = 0, Col = 4, IsActive = true }
				}
			};

			Assert.True(pattern.IsSymmetrical(SymmetryKind.Horizontal));
		}
	}
}
