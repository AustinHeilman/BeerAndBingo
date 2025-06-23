using Xunit;
using Bingo.Core.Domain.FlashBoard;

namespace Bingo.Core.Tests.Domain.FlashBoard;
public class FlashBoardObjTests
{
    [Fact]
    public void FlashBoard_ShouldInitializeWithFiveLetterGroups_AndFifteenNumbersEach()
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();

        Assert.Equal(5, board.Children.Count); // B, I, N, G, O

        foreach (var group in board.Children)
        {
            Assert.Equal(15, group.Cells.Count);
            Assert.All(group.Cells, cell =>
            {
                Assert.Equal(group, cell.Parent);
                Assert.Equal(board, cell.Board);
            });
        }
    }

    [Theory]
    [InlineData('B', 1)]
    [InlineData('I', 22)]
    [InlineData('N', 38)]
    [InlineData('G', 50)]
    [InlineData('O', 67)]
    public void GetCell_ShouldReturnCorrectCell(char letter, int number)
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();
        var cell = board.GetCell(letter, number);

        Assert.NotNull(cell);
        Assert.Equal(number, cell!.Number);
        Assert.Equal(letter, cell.Parent.Letter);
    }

    [Theory]
    [InlineData('B', 16)]
    [InlineData('Z', 5)]
    public void GetCell_ShouldReturnNull_IfOutOfRange(char letter, int number)
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();
        var cell = board.GetCell(letter, number);

        Assert.Null(cell);
    }

    [Fact]
    public void CallAndUncallNumber_ShouldToggleIsCalledFlag()
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();
        int number = 12;

        board.CallNumber(number);
        Assert.Contains(number, board.CalledNumbers);

        board.UncallNumber(number);
        Assert.DoesNotContain(number, board.CalledNumbers);
    }

    [Fact]
    public void CompletedColumns_ShouldReturnLetter_WhenGroupIsFullyCalled()
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();
        var group = board.Children.First(g => g.Letter == 'B');

        foreach (var cell in group.Cells)
            board.CallNumber(cell.Number);

        var completed = board.CompletedColumns().ToList();
        Assert.Single(completed);
        Assert.Equal('B', completed[0]);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(75)]
    public void IsValidNumber_ShouldReturnTrue_ForValidValues(int number)
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();
        Assert.True(board.IsValidNumber(number));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(76)]
    public void IsValidNumber_ShouldReturnFalse_ForOutOfBounds(int number)
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();
        Assert.False(board.IsValidNumber(number));
    }

    [Fact]
    public void Contains_ShouldReturnTrue_IfCellExists()
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();
        var exists = board.Contains('N', 40);
        Assert.True(exists);
    }

    [Fact]
    public void Contains_ShouldReturnFalse_IfCellDoesNotExist()
    {
        var board = new Bingo.Core.Domain.FlashBoard.FlashBoardObj();
        var exists = board.Contains('N', 99);
        Assert.False(exists);
    }
}
