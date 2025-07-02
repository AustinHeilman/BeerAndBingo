using Bingo.AppServices.Patterns;
using Bingo.Core.Domain.Bingo;
using Bingo.Core.FlashBoard;
using Bingo.ViewModel.FlashBoard;
using Bingo.ViewModel.GameInfo;
using Bingo.ViewModel.Patterns;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Bingo.ViewModel.MainPage.Caller;

public class CallerMainPageViewModel : INotifyPropertyChanged
{
	private readonly BingoSession _session = new();
	private readonly FlashBoardSyncService _sync;

	public GameInfoPanelViewModel GameInfoVM { get; }
	public InteractiveFlashBoardViewModel FlashBoardVM { get; }
	public PatternDisplayViewModel PatternVM { get; }

	public ICommand ToggleToolsPanelCommand { get; }
	public ICommand NextCallCommand { get; }
	public ICommand ReplayCommand { get; } = new RelayCommand(() => { });
	public ICommand UndoCommand { get; } = new RelayCommand(() => { });
	public ICommand RedoPickCommand { get; } = new RelayCommand(() => { });
	public ICommand PatternsCommand { get; } = new RelayCommand(() => { });
	public ICommand SettingsCommand { get; } = new RelayCommand(() => { });
	public ICommand NextGameCommand { get; }

	// Event to notify the view to show the NextRoundClock
	public event Action? ShowNextRoundClockRequested;

	public CallerMainPageViewModel()
	{
		_sync = new FlashBoardSyncService(_session);
		FlashBoardVM = new InteractiveFlashBoardViewModel(_sync.Board);
		GameInfoVM = new GameInfoPanelViewModel(_session);

		PatternVM = new PatternDisplayViewModel(new DefaultPatternRepository());
		_ = PatternVM.LoadPatternAsync("None");

		ToggleToolsPanelCommand = new RelayCommand(() => IsToolsPanelVisible = !IsToolsPanelVisible);
		NextCallCommand = new RelayCommand(() => _session.CallNext());
		NextGameCommand = new RelayCommand(OnNextGame);
	}

	private void OnNextGame()
	{
		ShowNextRoundClockRequested?.Invoke();
	}

	public void ResetSession()
	{
		_session.Restart();
	}

	private bool _isToolsPanelVisible;
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
				OnPropertyChanged(nameof(ToolsPanelToggleSymbol));
				OnPropertyChanged(nameof(ToolsPanelToggleIcon));
			}
		}
	}

	public string ToolsPanelToggleText => IsToolsPanelVisible ? "Hide Tools" : "Show Tools";
	public string ToolsPanelToggleSymbol => IsToolsPanelVisible ? "<<" : ">>";
	public string ToolsPanelToggleIcon => IsToolsPanelVisible ? "collapse" : "expand";

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
