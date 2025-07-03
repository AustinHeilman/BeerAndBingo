using Bingo.Core.Domain;
using Bingo.Core.FlashBoard;
using Bingo.Core.FlashBoard.Events;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Bingo.ViewModel.FlashBoard;

public class FlashBoardViewModel : INotifyPropertyChanged
{
	protected readonly FlashBoardObj _board;
	public ObservableCollection<FlashBoardGroupViewModel> Groups { get; } = new();

	public event Action<int, FlashBoardEventSource>? NumberCalledAnimationRequested;

	public ICommand ToggleCallCommand { get; }
	private readonly GameSessionState<int> _session;

	private bool _isInteractive = true;
	public bool IsInteractive => _isInteractive;

	public void SetInteractive(bool value)
	{
		if (_isInteractive != value)
		{
			_isInteractive = value;
			OnPropertyChanged(nameof(IsInteractive));
		}
	}

	public FlashBoardViewModel(GameSessionState<int> session, FlashBoardObj board)
	{
		_board = board;
		_session = session;
		ToggleCallCommand = new RelayCommand<int>(ToggleCalled);

		foreach (FlashBoardGroup group in _board.Children)
		{
			List<FlashBoardCellViewModel> cellVMs = group.Cells.Select(cell =>
			{
				FlashBoardCellViewModel vm = new(cell);
				cell.IsCalledChanged += (_, e) =>
				{
					NumberCalledAnimationRequested?.Invoke(cell.Number, e.SourceTag);
				};
				return vm;
			}).ToList();

			FlashBoardGroupViewModel groupVM = new(group.Letter, cellVMs);

			group.GroupCompleted += (_, _) =>
			{
				foreach (FlashBoardCellViewModel vm in groupVM.Cells)
					vm.GroupCompleted = true;
			};

			Groups.Add(groupVM);
		}
	}

	private void ToggleCalled(int number)
	{
		FlashBoardNumber? cell = _board.AllCells.FirstOrDefault(c => c.Number == number);
		if (cell is null)
			return;
		else if (!IsInteractive)
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

	public FlashBoardObj Model => _board;

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
