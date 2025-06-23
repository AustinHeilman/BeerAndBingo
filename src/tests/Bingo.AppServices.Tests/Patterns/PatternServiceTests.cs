using Bingo.AppServices.Patterns;
using Bingo.Core.Models;
using Bingo.Services.Patterns;
using Moq;

namespace Bingo.AppServices.Tests.Patterns;

public class PatternServiceTests
{
    [Fact]
    public async Task GetPatternNamesAsync_ReturnsAllNames()
    {
        var mockRepo = new Mock<IPatternRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new[]
        {
            new BingoPattern { Name = "Flag" },
            new BingoPattern { Name = "Smiley" }
        });

        var service = new PatternService(mockRepo.Object);

        var names = await service.GetPatternNamesAsync();

        Assert.Contains("Flag", names);
        Assert.Contains("Smiley", names);
        Assert.Equal(2, names.Count());
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsExpectedPattern()
    {
        var pattern = new BingoPattern { Name = "Cup" };

        var mockRepo = new Mock<IPatternRepository>();
        mockRepo.Setup(r => r.GetByNameAsync("Cup")).ReturnsAsync(pattern);

        var service = new PatternService(mockRepo.Object);
        var result = await service.GetByNameAsync("Cup");

        Assert.NotNull(result);
        Assert.Equal("Cup", result?.Name);
    }
}
