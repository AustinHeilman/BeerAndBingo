namespace Bingo.ViewModel.FlashBoard
{
    using Bingo.Core.Domain.FlashBoard.Events;

    public class FlashBoardCellViewModel
    {
        public int Number { get; }
        public char Letter { get; }

        public bool IsCalled { get; set; }
        public bool GroupCompleted { get; set; }
        public FlashBoardEventSource SourceTag { get; set; }

        public FlashBoardCellViewModel(int number, char letter)
        {
            Number = number;
            Letter = letter;
        }
    }
}
