namespace Bingo.ViewModel.FlashBoard
{
    public class FlashBoardGroupViewModel
    {
        public char Letter { get; }
        public List<FlashBoardCellViewModel> Cells { get; }

        public FlashBoardGroupViewModel(char letter, List<FlashBoardCellViewModel> cells)
        {
            Letter = letter;
            Cells = cells;
        }
    }
}
