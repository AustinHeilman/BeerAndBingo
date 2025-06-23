using Xunit;
using Bingo.Core.Domain.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;

namespace Bingo.Core.Tests.Domain.FlashBoard;

public class FlashBoardObjTests
{
    [Fact]
    public void CallAndUncallNumber_ShouldToggleState()
    {
        var board = new FlashBoardObj();
        int number = board.Children.First().Cells.First().Number;

        board.CallNumber(number, FlashBoardEventSource.Manual);
        Assert.Contains(number, board.CalledNumbers);

        board.UncallNumber(number, FlashBoardEventSource.Manual);
        Assert.DoesNotContain(number, board.CalledNumbers);
    }
}
