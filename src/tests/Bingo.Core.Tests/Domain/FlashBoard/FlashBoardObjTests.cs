using Bingo.Core.Games.Bingo.FlashBoard;
using Bingo.Core.Games.Bingo.FlashBoard.Events;

namespace Bingo.Core.Tests.Domain.FlashBoard;

public class FlashBoardObjTests
{
	private readonly FlashBoardConfig _config = new();
	[Fact]
	public void CallAndUncallNumber_ShouldToggleState()
	{
		FlashBoardObj board = new(_config);
		int number = board.Children.First().Cells.First().Number;

		board.CallNumber(number, FlashBoardEventSource.Manual);
		Assert.Contains(number, board.CalledNumbers);

		board.UncallNumber(number, FlashBoardEventSource.Manual);
		Assert.DoesNotContain(number, board.CalledNumbers);
	}
}
