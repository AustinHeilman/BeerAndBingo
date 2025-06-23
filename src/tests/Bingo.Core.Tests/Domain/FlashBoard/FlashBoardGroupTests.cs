using Xunit;
using Bingo.Core.Domain.FlashBoard;

namespace Bingo.Core.Tests.Domain.FlashBoard;

public class FlashBoardGroupTests
{
    [Fact]
    public void GroupCompleted_ShouldFire_WhenAllNumbersAreCalled()
    {
        var board = new FlashBoardObj();
        var group = board.Children.First();
        char? completed = null;

        group.GroupCompleted += (_, letter) => completed = letter;

        foreach (var number in group.Cells)
            number.IsCalled = true;

        Assert.Equal(group.Letter, completed);
    }

    [Fact]
    public void GroupCompleted_ShouldNotFire_WhenNotAllNumbersAreCalled()
    {
        var board = new FlashBoardObj();
        var group = board.Children.First();
        bool fired = false;

        group.GroupCompleted += (_, _) => fired = true;

        for (int i = 0; i < group.Cells.Count - 1; i++)
            group.Cells[i].IsCalled = true;

        Assert.False(fired);
    }
}
