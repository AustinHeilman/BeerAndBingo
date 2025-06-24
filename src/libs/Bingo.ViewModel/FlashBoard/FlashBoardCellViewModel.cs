using Bingo.Core.Domain.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;

namespace Bingo.ViewModel.FlashBoard
{
    public class FlashBoardCellViewModel
    {
        private readonly FlashBoardNumber _model;

        public FlashBoardCellViewModel(FlashBoardNumber model)
        {
            _model = model;
            _model.IsCalledChanged += (s, e) =>
            {
                IsCalled = e.NewValue;
                SourceTag = e.SourceTag;
            };
        }

        public int Number => _model.Number;
        public char Letter => _model.Parent.Letter;

        public bool IsCalled { get; private set; } = false;
        public bool GroupCompleted { get; set; } = false;
        public FlashBoardEventSource SourceTag { get; private set; } = FlashBoardEventSource.Unknown;

        public FlashBoardNumber Model => _model;
    }
}
