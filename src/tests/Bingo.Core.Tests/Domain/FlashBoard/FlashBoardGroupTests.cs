using Bingo.Core.Games.Bingo.FlashBoard;
using Bingo.Core.Games.Bingo.FlashBoard.Events;

namespace Bingo.Core.Tests.Domain.FlashBoard;
public class FlashBoardGroupTests
{
	private readonly FlashBoardConfig FlashBoardConfig = new();

	[Fact]
	public void GroupCompleted_ShouldFire_WhenAllNumbersAreCalled()
	{
		FlashBoardObj board = new(FlashBoardConfig);
		FlashBoardGroup group = board.Children.First();
		char? completed = null;

		group.GroupCompleted += (_, letter) => completed = letter;

		foreach (FlashBoardNumber number in group.Cells)
			number.SetCalled(true, FlashBoardEventSource.Manual);

		Assert.Equal(group.Letter, completed);
	}

	[Fact]
	public void GroupCompleted_ShouldNotFire_WhenNotAllNumbersAreCalled()
	{
		FlashBoardObj board = new(FlashBoardConfig);
		FlashBoardGroup group = board.Children.First();
		bool fired = false;

		group.GroupCompleted += (_, _) => fired = true;

		for (int i = 0; i < group.Cells.Count - 1; i++)
			group.Cells[i].SetCalled(true, FlashBoardEventSource.Manual);

		Assert.False(fired);
	}
}
