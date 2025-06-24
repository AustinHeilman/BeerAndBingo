using Bingo.Core.Domain.FlashBoard.Events;
using Bingo.Services.FlashBoard;

namespace Bingo.Services.Tests.FlashBoard;

public class FlashBoardService_EventTests
{
    [Fact]
    public void NumberCalledChanged_ShouldFire_WhenStateChanges()
    {
        FlashBoardService service = new();
        FlashBoardCalledChangedEventArgs? received = null;

        service.NumberCalledChanged += (_, args) => received = args;

        service.CallNumber(10);
        Assert.NotNull(received);
        Assert.Equal(10, received!.Source.Number);
    }

    [Fact]
    public void GroupCompleted_ShouldFire_WhenColumnIsFullyCalled()
    {
        FlashBoardService service = new();
        char? completed = null;

        service.GroupCompleted += (_, letter) => completed = letter;
        Core.Domain.FlashBoard.FlashBoardGroup group = service.Board.Children.First();

        foreach (Core.Domain.FlashBoard.FlashBoardNumber cell in group.Cells)
            service.CallNumber(cell.Number);

        Assert.Equal(group.Letter, completed);
    }
}
