using Bingo.AppServices.Patterns;
using Bingo.Core.Domain;
using Bingo.Core.Domain.Bingo;
using Bingo.Core.FlashBoard;
using Bingo.Core.FlashBoard.Events;
using Bingo.ViewModel.FlashBoard;
using Bingo.ViewModel.GameInfo;
using Bingo.ViewModel.Helpers;
using Bingo.ViewModel.Patterns;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Bingo.ViewModel.MainPage.Caller;

public class CallerMainPageViewModel : INotifyPropertyChanged
{
	private readonly GameSessionState<int> _session = new BingoSession();
	private readonly FlashBoardSyncService _sync;

	public GameInfoPanelViewModel GameInfoVM { get; }

	public InteractiveFlashBoardViewModel FlashBoardVM { get; }
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
				OnPropertyChanged(nameof(ToolsPanelToggleSymbol));
				OnPropertyChanged(nameof(ToolsPanelToggleIcon));
			}
		}
	}

	public string ToolsPanelToggleText => IsToolsPanelVisible ? "Hide Tools" : "Show Tools";
	public string ToolsPanelToggleSymbol => IsToolsPanelVisible ? "<<" : ">>";
	public string ToolsPanelToggleIcon => IsToolsPanelVisible ? "collapse" : "expand";

	public ICommand ToggleToolsPanelCommand { get; }
	public ICommand NextCallCommand { get; }
	public ICommand ReplayCommand { get; } = new RelayCommand(() => { /* TBD */ });
	public ICommand UndoCommand { get; } = new RelayCommand(() => { /* TBD */ });
	public ICommand RedoPickCommand { get; } = new RelayCommand(() => { /* TBD */ });
	public ICommand PatternsCommand { get; } = new RelayCommand(() => { /* TBD */ });
	public ICommand SettingsCommand { get; } = new RelayCommand(() => { /* TBD */ });

	public CallerMainPageViewModel()
	{
		_sync = new FlashBoardSyncService(_session);
		FlashBoardVM = new InteractiveFlashBoardViewModel(_sync.Board);

		DefaultPatternRepository repo = new();
		PatternVM = new PatternDisplayViewModel(repo);
		_ = PatternVM.LoadPatternAsync("None");

		GameInfoVM = new GameInfoPanelViewModel(_session);

		ToggleToolsPanelCommand = new RelayCommand(() => IsToolsPanelVisible = !IsToolsPanelVisible);
		NextCallCommand = new RelayCommand(() => _session.CallNext());
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
