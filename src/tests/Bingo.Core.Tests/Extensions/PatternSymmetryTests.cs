using System.Collections.Generic;
using Xunit;
using Bingo.Core.Patterns;
using Bingo.Core.Extensions;
using static Bingo.Core.Extensions.IsSymmetricalExtension;

namespace Bingo.Core.Tests.Extensions
{
	public class PatternSymmetryTests
	{
		[Fact]
		public void Diagonal_Symmetry_Is_Detected()
		{
			var pattern = new BingoPattern
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
			var pattern = new BingoPattern
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
