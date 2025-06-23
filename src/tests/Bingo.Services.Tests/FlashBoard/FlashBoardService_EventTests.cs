using Xunit;
using Bingo.Services.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;

namespace Bingo.Services.Tests.FlashBoard;

public class FlashBoardService_EventTests
{
    [Fact]
    public void NumberCalledChanged_ShouldFire_WhenStateChanges()
    {
        var service = new FlashBoardService();
        FlashBoardCalledChangedEventArgs? received = null;

        service.NumberCalledChanged += (_, args) => received = args;

        service.CallNumber(10);
        Assert.NotNull(received);
        Assert.Equal(10, received!.Source.Number);
    }

    [Fact]
    public void GroupCompleted_ShouldFire_WhenColumnIsFullyCalled()
    {
        var service = new FlashBoardService();
        char? completed = null;

        service.GroupCompleted += (_, letter) => completed = letter;
        var group = service.Board.Children.First();

        foreach (var cell in group.Cells)
            service.CallNumber(cell.Number);

        Assert.Equal(group.Letter, completed);
    }
}
