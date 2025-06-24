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
        Mock<IPatternRepository> mockRepo = new();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new[]
        {
            new BingoPattern { Name = "Flag" },
            new BingoPattern { Name = "Smiley" }
        });

        PatternService service = new(mockRepo.Object);

        IEnumerable<string> names = await service.GetPatternNamesAsync();

        Assert.Contains("Flag", names);
        Assert.Contains("Smiley", names);
        Assert.Equal(2, names.Count());
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsExpectedPattern()
    {
        BingoPattern pattern = new() { Name = "Cup" };

        Mock<IPatternRepository> mockRepo = new();
        mockRepo.Setup(r => r.GetByNameAsync("Cup")).ReturnsAsync(pattern);

        PatternService service = new(mockRepo.Object);
        BingoPattern? result = await service.GetByNameAsync("Cup");

        Assert.NotNull(result);
        Assert.Equal("Cup", result?.Name);
    }
}
