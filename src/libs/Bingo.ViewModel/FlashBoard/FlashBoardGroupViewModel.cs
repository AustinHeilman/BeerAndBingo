using System.Diagnostics;

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

		public void SetParentBoard(FlashBoardViewModel board)
		{
			foreach (var cell in Cells)
			{
				cell.SetParentBoard(board);
				if (cell is FlashBoardCellViewModel vm)
					if ( vm.Number == 67) // Debugging specific cell
						Debug.WriteLine($"[FlashBoardGroupViewModel.SetParentBoard] Cell {vm.Number} now listening to board {board}");				
			}
		}
	}
}
