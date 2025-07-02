using Bingo.Core.FlashBoard;
using Bingo.Core.FlashBoard.Events;

public class FlashBoardCalledChangedEventArgs : EventArgs
{
	public FlashBoardNumber Source { get; }
	public bool NewValue { get; }
	public bool OldValue { get; }
	public FlashBoardEventSource SourceTag { get; }

	public FlashBoardCalledChangedEventArgs(FlashBoardNumber source, bool oldValue, bool newValue, FlashBoardEventSource sourceTag)
	{
		Source = source;
		NewValue = newValue;
		OldValue = oldValue;
		SourceTag = sourceTag;
	}
}
