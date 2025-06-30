using Bingo.AppServices.Patterns;

namespace Bingo.AppServices.Tests.Patterns
{
	public class DefaultPatternRepositoryTests
	{
		[Fact]
		public async Task Can_Load_Default_Patterns()
		{
			DefaultPatternRepository repo = new();

			IEnumerable<Core.Games.Bingo.Patterns.BingoPattern> all = await repo.GetAllAsync();

			Assert.NotNull(all);
			Assert.Contains(all, p => p.Name == "4 Corners");
		}
	}
}
