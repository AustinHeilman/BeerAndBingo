using Bingo.Core.Domain.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;
using Bingo.Services.FlashBoard;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bingo.ViewModel.FlashBoard
{
    public class FlashBoardViewModel
    {
        private readonly FlashBoardService _service = new();

        public ObservableCollection<FlashBoardCellViewModel> Cells { get; } = new();

        public event Action<int, FlashBoardEventSource>? NumberCalledAnimationRequested;
        public IRelayCommand<int> ToggleCallCommand { get; }
        public IReadOnlyList<FlashBoardGroup> Groups => _service.BoardGroups;

        public FlashBoardViewModel()
        {
            foreach (var group in _service.Board.Children)
            {
                foreach (var number in group.Cells)
                {
                    Cells.Add(new FlashBoardCellViewModel(number));
                }
            }
            ToggleCallCommand = new RelayCommand<int>(ToggleCallNumber);
            _service.NumberCalledChanged += OnNumberCalledChanged;
            _service.GroupCompleted += OnGroupCompleted;
        }

        public IReadOnlyList<int> CalledNumbers => _service.CalledNumbers;
        public IEnumerable<char> CompletedColumns => _service.Board.Children
        .Where(group => group.Cells.All(cell => cell.IsCalled))
        .Select(group => group.Letter);

        private void OnNumberCalledChanged(object? sender, FlashBoardCalledChangedEventArgs e)
        {
            var vm = Cells.FirstOrDefault(c => c.Number == e.Source.Number);
            if (vm is not null)
            {
                NumberCalledAnimationRequested?.Invoke(vm.Number, e.SourceTag);
            }
        }

        private void OnGroupCompleted(object? sender, char letter)
        {
            foreach (var vm in Cells.Where(c => c.Letter == letter))
            {
                vm.GroupCompleted = true;
            }
        }

        public void CallNumber(int number) => _service.CallNumber(number, FlashBoardEventSource.Manual);

        public void UncallNumber(int number) => _service.UncallNumber(number);
        public void LoadSnapshot(FlashBoardSnapshot snapshot) => _service.LoadSnapshot(snapshot);

        private void ToggleCallNumber(int number)
        {
            if (_service.CalledNumbers.Contains(number))
                _service.UncallNumber(number);
            else
                _service.CallNumber(number);
        }
    }
}
