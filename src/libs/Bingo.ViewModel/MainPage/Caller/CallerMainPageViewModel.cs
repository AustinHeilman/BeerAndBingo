using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Bingo.AppServices.Patterns;
using Bingo.ViewModel.FlashBoard;
using Bingo.ViewModel.Patterns;
using Bingo.ViewModel.Helpers; // Add this for DelegateCommand

namespace Bingo.ViewModel.MainPage.Caller;

public class CallerMainPageViewModel : INotifyPropertyChanged
{
    public FlashBoardViewModel FlashBoardVM { get; } = new();
    public PatternDisplayViewModel PatternVM { get; }

    private bool _isToolsPanelVisible = false;
    public bool IsToolsPanelVisible
    {
        get => _isToolsPanelVisible;
        set
        {
            if (_isToolsPanelVisible != value)
            {
                _isToolsPanelVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ToolsPanelToggleText));
            }
        }
    }

    public string ToolsPanelToggleText => IsToolsPanelVisible ? "Hide Tools" : "Show Tools";

    public ICommand ToggleToolsPanelCommand { get; }

    // Placeholder commands for the IconButtons
    public ICommand NextCallCommand { get; } = new DelegateCommand(() => { /* Implement logic */ });
    public ICommand ReplayCommand { get; } = new DelegateCommand(() => { /* Implement logic */ });
    public ICommand UndoCommand { get; } = new DelegateCommand(() => { /* Implement logic */ });

    public CallerMainPageViewModel()
    {
        DefaultPatternRepository repo = new();
        PatternVM = new PatternDisplayViewModel(repo);

        // Ensure a visible grid with no active cells on launch
        _ = PatternVM.LoadPatternAsync("None");

        ToggleToolsPanelCommand = new DelegateCommand(() => IsToolsPanelVisible = !IsToolsPanelVisible);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
