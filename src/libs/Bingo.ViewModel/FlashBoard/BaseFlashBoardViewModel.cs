using System.Collections.ObjectModel;
using System.ComponentModel;
using Bingo.Core.FlashBoard;
using Bingo.Core.FlashBoard.Events;

namespace Bingo.ViewModel.FlashBoard;

public abstract class BaseFlashBoardViewModel : INotifyPropertyChanged
{
	protected readonly FlashBoardObj _board;
	public ObservableCollection<FlashBoardGroupViewModel> Groups { get; } = new();

	public event Action<int, FlashBoardEventSource>? NumberCalledAnimationRequested;

	protected BaseFlashBoardViewModel(FlashBoardObj board)
	{
		_board = board;

		foreach (FlashBoardGroup group in _board.Children)
		{
			var cellVMs = group.Cells.Select(cell =>
			{
				var vm = new FlashBoardCellViewModel(cell);
				cell.IsCalledChanged += (_, e) =>
				{
					NumberCalledAnimationRequested?.Invoke(cell.Number, e.SourceTag);
				};
				return vm;
			}).ToList();

			var groupVM = new FlashBoardGroupViewModel(group.Letter, cellVMs);

			group.GroupCompleted += (_, _) =>
			{
				foreach (var vm in groupVM.Cells)
					vm.GroupCompleted = true;
			};

			Groups.Add(groupVM);
		}
	}

	public FlashBoardObj Model => _board;

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
