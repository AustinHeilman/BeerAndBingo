namespace Bingo.Core.Domain.FlashBoard.Events
{
	public class FlashBoardCalledChangedEventArgs : EventArgs
	{
		public FlashBoardNumber Source { get; }
		public bool OldValue { get; }
		public bool NewValue { get; }
		public FlashBoardEventSource SourceTag { get; }

		public FlashBoardCalledChangedEventArgs(
			FlashBoardNumber source,
			bool oldValue,
			bool newValue,
			FlashBoardEventSource sourceTag)
		{
			Source = source;
			OldValue = oldValue;
			NewValue = newValue;
			SourceTag = sourceTag;
		}
	}
}
