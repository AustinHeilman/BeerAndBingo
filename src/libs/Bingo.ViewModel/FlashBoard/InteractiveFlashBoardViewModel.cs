using Bingo.Core.Domain;
using Bingo.Core.FlashBoard;
using Bingo.Core.FlashBoard.Events;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Bingo.ViewModel.FlashBoard;

public class InteractiveFlashBoardViewModel : BaseFlashBoardViewModel
{
	public ICommand ToggleCallCommand { get; }
	private readonly GameSessionState<int> _session;

	public InteractiveFlashBoardViewModel(GameSessionState<int> session, FlashBoardObj board) : base(board)
	{
		_session = session;
		ToggleCallCommand = new RelayCommand<int>(ToggleCalled);
	}

	private void ToggleCalled(int number)
	{
		FlashBoardNumber? cell = _board.AllCells.FirstOrDefault(c => c.Number == number);
		if (cell is null)
			return;

		if (!cell.IsCalled)
		{
			_session.CallItem(cell.Number);
		}
		else
		{
			_session.UncallItem(cell.Number);			
		}
	}
}
