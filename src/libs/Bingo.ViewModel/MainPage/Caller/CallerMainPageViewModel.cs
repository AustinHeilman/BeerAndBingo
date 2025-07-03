using Bingo.AppServices.Patterns;
using Bingo.Core.Domain.Bingo;
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
	public ICommand ReplayCommand { get; }
	public ICommand UndoPickCommand { get; }
	public ICommand RedoPickCommand { get; }
	public ICommand PatternsCommand { get; }
	public ICommand SettingsCommand { get; }
	public ICommand NextGameCommand { get; }

	public event Action? ShowNextRoundClockRequested;

	public CallerMainPageViewModel()
	{
		_sync = new FlashBoardSyncService(_session);
		FlashBoardVM = new InteractiveFlashBoardViewModel(_session, _sync.Board);
		GameInfoVM = new GameInfoPanelViewModel(_session);
		PatternVM = new PatternDisplayViewModel(new DefaultPatternRepository());

		_ = PatternVM.LoadPatternAsync("None");

		ToggleToolsPanelCommand = new RelayCommand(() => IsToolsPanelVisible = !IsToolsPanelVisible);
		NextCallCommand = new RelayCommand(() => _session.CallNext());
		NextGameCommand = new RelayCommand(OnNextGame);

		UndoPickCommand = new RelayCommand(_session.Undo);
		RedoPickCommand = new RelayCommand(_session.Redo);
		ReplayCommand = new RelayCommand(() => { /* Coming soon 👀 */ });
		PatternsCommand = new RelayCommand(() => { /* TODO */ });
		SettingsCommand = new RelayCommand(() => { /* TODO */ });

		// Optional: Subscribe to session change events
		_session.ItemCalled += (_, _) => NotifySessionUpdate();
		_session.UndoPerformed += (_, _) => NotifySessionUpdate();
		_session.RedoPerformed += (_, _) => NotifySessionUpdate();
	}

	private void OnNextGame()
	{
		ShowNextRoundClockRequested?.Invoke();
	}

	public void ResetSession()
	{
		_session.Restart();
		NotifySessionUpdate();
	}

	private void NotifySessionUpdate()
	{
		OnPropertyChanged(nameof(UndoPickCommand));
		OnPropertyChanged(nameof(RedoPickCommand));

		// Optional: Trigger any manual updates for UI-bound properties
		// FlashBoardVM.NotifyChange();
		// GameInfoVM.NotifyChange();
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
				OnPropertyChanged(nameof(ToolsPanelToggleIcon));
			}
		}
	}

	public string ToolsPanelToggleIcon => IsToolsPanelVisible ? "collapse" : "expand";

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
