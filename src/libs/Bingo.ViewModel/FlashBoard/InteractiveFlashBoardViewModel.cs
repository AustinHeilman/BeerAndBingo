using Bingo.Core.FlashBoard;
using Bingo.Core.FlashBoard.Events;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Bingo.ViewModel.FlashBoard;

public class InteractiveFlashBoardViewModel : BaseFlashBoardViewModel
{
	public ICommand ToggleCallCommand { get; }

	public InteractiveFlashBoardViewModel(FlashBoardObj board) : base(board)
	{
		ToggleCallCommand = new RelayCommand<int>(ToggleCalled);
	}

	private void ToggleCalled(int number)
	{
		FlashBoardNumber? cell = _board.AllCells.FirstOrDefault(c => c.Number == number);
		if (cell is not null)
		{
			bool newState = !cell.IsCalled;
			cell.SetCalled(newState, FlashBoardEventSource.Manual);
		}
	}
}
