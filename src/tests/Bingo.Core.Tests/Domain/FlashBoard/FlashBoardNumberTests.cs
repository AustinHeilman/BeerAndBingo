using Xunit;
using Bingo.Core.Domain.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;

namespace Bingo.Core.Tests.Domain.FlashBoard;

public class FlashBoardNumberTests
{
    [Fact]
    public void IsCalled_ShouldRaiseEvent_WhenValueChanges()
    {
        var group = new FlashBoardGroup('B', 1, 15, new FlashBoardObj());
        var number = group.Cells.First();
        FlashBoardCalledChangedEventArgs? capturedEvent = null;

        number.IsCalledChanged += (s, e) => capturedEvent = e;

        number.IsCalled = true;

        Assert.NotNull(capturedEvent);
        Assert.True(capturedEvent!.NewValue);
        Assert.False(capturedEvent.OldValue);
        Assert.Equal(number, capturedEvent.Source);
    }

    [Fact]
    public void IsCalled_ShouldNotRaiseEvent_WhenValueIsSame()
    {
        var group = new FlashBoardGroup('B', 1, 15, new FlashBoardObj());
        var number = group.Cells.First();

        bool wasRaised = false;
        number.IsCalledChanged += (_, _) => wasRaised = true;

        number.IsCalled = false; // default value
        Assert.False(wasRaised);
    }
}
