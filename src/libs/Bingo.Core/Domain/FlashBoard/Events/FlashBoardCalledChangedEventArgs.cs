using System;

namespace Bingo.Core.Domain.FlashBoard.Events
{
    public class FlashBoardCalledChangedEventArgs : EventArgs
    {
        public FlashBoardNumber Source { get; }
        public bool OldValue { get; }
        public bool NewValue { get; }

        public FlashBoardCalledChangedEventArgs(FlashBoardNumber source, bool oldValue, bool newValue)
        {
            Source = source;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
