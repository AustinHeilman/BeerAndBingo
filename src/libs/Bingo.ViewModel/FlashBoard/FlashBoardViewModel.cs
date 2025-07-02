using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Bingo.Core.FlashBoard;
using Bingo.Core.FlashBoard.Events;
using Bingo.ViewModel.Helpers;
using CommunityToolkit.Mvvm.Input;


namespace Bingo.ViewModel.FlashBoard;

public class FlashBoardViewModel : INotifyPropertyChanged
{
	private readonly FlashBoardObj _board;

	public ObservableCollection<FlashBoardGroupViewModel> Groups { get; } = new();

	/// <summary>
	/// Fired when a call animation should trigger from external state (not user input).
	/// </summary>
	public event Action<int, FlashBoardEventSource>? NumberCalledAnimationRequested;

	public ICommand ToggleCallCommand { get; }

	public FlashBoardViewModel(FlashBoardObj board)
	{
		_board = board;

		foreach (FlashBoardGroup group in _board.Children)
		{
			var cellVMs = group.Cells
				.Select(cell =>
				{
					var vm = new FlashBoardCellViewModel(cell);

					// Relay animation requests from model events
					cell.IsCalledChanged += (s, e) =>
					{
						NumberCalledAnimationRequested?.Invoke(cell.Number, e.SourceTag);
					};

					return vm;
				})
				.ToList();

			var groupVM = new FlashBoardGroupViewModel(group.Letter, cellVMs);

			// Update group completion state
			group.GroupCompleted += (_, letter) =>
			{
				if (letter == groupVM.Letter)
				{
					foreach (var vm in groupVM.Cells)
						vm.GroupCompleted = true;
				}
			};

			Groups.Add(groupVM);
		}

		ToggleCallCommand = new RelayCommand<int>(ToggleCalled);
	}

	private void ToggleCalled(int number)
	{
		var cell = _board.AllCells.FirstOrDefault(c => c.Number == number);
		if (cell is not null)
		{
			bool next = !cell.IsCalled;
			cell.SetCalled(next, FlashBoardEventSource.Manual);
		}
	}

	public FlashBoardObj Model => _board;

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
