using Bingo.Core.Domain.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;

namespace Bingo.Core.Tests.Domain.FlashBoard;

public class FlashBoardNumberTests
{
	[Fact]
	public void SetCalled_ShouldRaiseEvent_WhenValueChanges()
	{
		FlashBoardGroup group = new('B', 1, 15, new FlashBoardObj());
		FlashBoardNumber number = group.Cells.First();
		FlashBoardCalledChangedEventArgs? capturedEvent = null;

		number.IsCalledChanged += (_, e) => capturedEvent = e;

		number.SetCalled(true, FlashBoardEventSource.Manual);

		Assert.NotNull(capturedEvent);
		Assert.True(capturedEvent!.NewValue);
		Assert.False(capturedEvent.OldValue);
		Assert.Equal(number, capturedEvent.Source);
		Assert.Equal(FlashBoardEventSource.Manual, capturedEvent.SourceTag);
	}

	[Fact]
	public void SetCalled_ShouldNotRaiseEvent_WhenValueIsSame()
	{
		FlashBoardGroup group = new('B', 1, 15, new FlashBoardObj());
		FlashBoardNumber number = group.Cells.First();
		bool wasRaised = false;

		number.IsCalledChanged += (_, _) => wasRaised = true;

		number.SetCalled(false, FlashBoardEventSource.Manual);

		Assert.False(wasRaised);
	}
}
