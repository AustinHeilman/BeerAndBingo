using Bingo.Core.Domain.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;
using Bingo.Services.FlashBoard;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Bingo.ViewModel.FlashBoard
{
	public class FlashBoardViewModel
	{
		private readonly FlashBoardService _service = new();

		public ObservableCollection<FlashBoardGroupViewModel> Groups { get; } = new();

		public event Action<int, FlashBoardEventSource>? NumberCalledAnimationRequested;
		public IRelayCommand<int> ToggleCallCommand { get; }

		public FlashBoardViewModel()
		{
			foreach (FlashBoardGroup group in _service.Board.Children)
			{
				List<FlashBoardCellViewModel> cellVMs = group.Cells
					.OrderBy(c => c.Number)
					.Select(c => new FlashBoardCellViewModel(c))
					.ToList();

				Groups.Add(new FlashBoardGroupViewModel(group.Letter, cellVMs));
			}

			ToggleCallCommand = new RelayCommand<int>(ToggleCallNumber);

			_service.NumberCalledChanged += OnNumberCalledChanged;
			_service.GroupCompleted += OnGroupCompleted;
		}

		public IReadOnlyList<int> CalledNumbers => _service.CalledNumbers;

		public IEnumerable<char> CompletedColumns =>
			_service.Board.Children
					 .Where(g => g.Cells.All(c => c.IsCalled))
					 .Select(g => g.Letter);

		private void OnNumberCalledChanged(object? sender, FlashBoardCalledChangedEventArgs e)
		{
			FlashBoardCellViewModel? cellVM = Groups
				.SelectMany(r => r.Cells)
				.FirstOrDefault(c => c.Number == e.Source.Number);

			if (cellVM is not null)
			{
				NumberCalledAnimationRequested?.Invoke(cellVM.Number, e.SourceTag);
			}
		}

		private void OnGroupCompleted(object? sender, char letter)
		{
			FlashBoardGroupViewModel? row = Groups.FirstOrDefault(r => r.Letter == letter);
			if (row is not null)
			{
				foreach (FlashBoardCellViewModel cell in row.Cells)
					cell.GroupCompleted = true;
			}
		}

		public void CallNumber(int number) =>
			_service.CallNumber(number, FlashBoardEventSource.Manual);

		public void UncallNumber(int number) =>
			_service.UncallNumber(number);

		public void LoadSnapshot(FlashBoardSnapshot snapshot) =>
			_service.LoadSnapshot(snapshot);

		private void ToggleCallNumber(int number)
		{
			if (CalledNumbers.Contains(number))
				UncallNumber(number);
			else
				CallNumber(number);
		}
	}
}
