using Xunit;
using Bingo.Services.FlashBoard;
using Bingo.Core.Domain.FlashBoard;
using Bingo.Core.Domain;

namespace Bingo.Tests.Services.FlashBoard;

public class FlashBoardServiceTests
{
    [Fact]
    public void NewGame_ShouldClearCalledNumbersAndHistory()
    {
        var service = new FlashBoardService();
        service.CallNumber(10);
        service.CallNumber(25);

        service.NewGame();

        Assert.Empty(service.CalledNumbers);
        Assert.Empty(service.ActionHistory);
    }

    [Fact]
    public void CallNumber_ShouldAddToCalledNumbers_AndTrackHistory()
    {
        var service = new FlashBoardService();

        service.CallNumber(10);
        service.CallNumber(20);

        Assert.Contains(10, service.CalledNumbers);
        Assert.Contains(20, service.CalledNumbers);

        var history = service.ActionHistory.ToList();
        Assert.Equal(2, history.Count);
        Assert.Equal(("Call", 10), history[0]);
        Assert.Equal(("Call", 20), history[1]);
    }

    [Fact]
    public void UncallNumber_ShouldRemoveFromCalledNumbers_AndTrackHistory()
    {
        var service = new FlashBoardService();

        service.CallNumber(33);
        service.UncallNumber(33);

        Assert.DoesNotContain(33, service.CalledNumbers);

        var history = service.ActionHistory.ToList();
        Assert.Equal(("Call", 33), history[0]);
        Assert.Equal(("Uncall", 33), history[1]);
    }

    [Fact]
    public void GetAvailableNumbers_ShouldExcludeCalledNumbers()
    {
        var service = new FlashBoardService();
        service.CallNumber(5);
        service.CallNumber(10);

        var available = service.GetAvailableNumbers().ToList();

        Assert.DoesNotContain(5, available);
        Assert.DoesNotContain(10, available);
        Assert.Equal(FlashBoardConfig.TotalNumbers - 2, available.Count);
    }

    [Fact]
    public void GetAvailableNumbers_WithScope_ShouldOnlyIncludeLettersAndUncalled()
    {
        var service = new FlashBoardService();
        service.CallNumber(1);   // B
        service.CallNumber(61);  // O

        var scoped = service.GetAvailableNumbers(new[] { 'B', 'O' }).ToList();

        Assert.DoesNotContain(1, scoped);
        Assert.DoesNotContain(61, scoped);
        Assert.All(scoped, n =>
        {
            var letter = FlashBoardConfig.GetLetterForNumber(n);
            Assert.True(letter == 'B' || letter == 'O');
        });
    }

    [Fact]
    public void PickRandomAvailableNumber_ShouldReturnNull_WhenEmpty()
    {
        var service = new FlashBoardService();

        for (int i = 1; i <= FlashBoardConfig.TotalNumbers; i++)
            service.CallNumber(i);

        var pick = service.PickRandomAvailableNumber();
        Assert.Null(pick);
    }

    [Fact]
    public void PickRandomAvailableNumber_ShouldReturnNumber_WhenAvailable()
    {
        var service = new FlashBoardService();

        var pick = service.PickRandomAvailableNumber();
        Assert.NotNull(pick);
        Assert.InRange(pick.Value, 1, 75);
    }
}
