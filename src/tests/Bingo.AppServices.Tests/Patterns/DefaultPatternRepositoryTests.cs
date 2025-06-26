using System.Threading.Tasks;
using Xunit;
using Bingo.AppServices.Patterns;
using Bingo.Core.Patterns;

namespace Bingo.AppServices.Tests.Patterns
{
	public class DefaultPatternRepositoryTests
	{
		[Fact]
		public async Task Can_Load_Default_Patterns()
		{
			var repo = new DefaultPatternRepository();

			var all = await repo.GetAllAsync();

			Assert.NotNull(all);
			Assert.Contains(all, p => p.Name == "4 Corners");
		}
	}
}
