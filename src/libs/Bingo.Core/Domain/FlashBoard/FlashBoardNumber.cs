using Bingo.Core.Domain.FlashBoard.Events;
using System;

namespace Bingo.Core.Domain.FlashBoard
{
    public class FlashBoardNumber
    {
        public int Number { get; }
        public FlashBoardGroup Parent { get; internal set; }
        public FlashBoardObj? Board => Parent?.Parent;
        public int ColumnIndex => Board?.Children.IndexOf(Parent) ?? -1;

        private bool _isCalled;

        public event EventHandler<FlashBoardCalledChangedEventArgs>? IsCalledChanged;

        public bool IsCalled
        {
            get => _isCalled;
            set
            {
                if (_isCalled != value)
                {
                    bool oldValue = _isCalled;
                    _isCalled = value;
                    IsCalledChanged?.Invoke(this, new FlashBoardCalledChangedEventArgs(this, oldValue, _isCalled));
                }
            }
        }

        public FlashBoardNumber(int number, FlashBoardGroup parent)
        {
            Number = number;
            Parent = parent;
            _isCalled = false;
        }

        public override bool Equals(object? obj) =>
            obj is FlashBoardNumber other && Number == other.Number;

        public override int GetHashCode() => Number.GetHashCode();
    }
}
