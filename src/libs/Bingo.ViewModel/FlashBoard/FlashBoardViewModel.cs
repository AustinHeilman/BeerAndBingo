using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Bingo.ModelView.FlashBoard;

public partial class FlashBoardViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<int> calledNumbers = new();

    [RelayCommand]
    private void ToggleCall(int number)
    {
        if (CalledNumbers.Contains(number))
            CalledNumbers.Remove(number);
        else
            CalledNumbers.Add(number);

        OnPropertyChanged(nameof(CompletedColumns));
    }

    public IEnumerable<char> CompletedColumns
    {
        get
        {
            var completed = new List<char>();

            for (int col = 0; col < 5; col++)
            {
                var columnNumbers = Enumerable.Range(0, 5)
                    .Select(row => col + (row * 15) + 1);

                if (columnNumbers.All(CalledNumbers.Contains))
                    completed.Add((char)('B' + col));
            }

            return completed;
        }
    }
}
