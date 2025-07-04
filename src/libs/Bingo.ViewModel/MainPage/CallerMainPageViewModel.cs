using Bingo.Core.Domain.Bingo;
using Bingo.Core.FlashBoard.Events;
using Bingo.Core.Patterns;
using Bingo.ViewModel.FlashBoard;
using Bingo.ViewModel.GameInfo;
using Bingo.ViewModel.Patterns;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Bingo.ViewModel.MainPage;

public class CallerMainPageViewModel : INotifyPropertyChanged
{
	private readonly BingoSession _session = new();
	private readonly FlashBoardSyncService _sync;
	private readonly PatternRepositoryBase _repository;

	public GameInfoPanelViewModel GameInfoVM { get; }
	public FlashBoardViewModel FlashBoardVM { get; }
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

	private CancellationTokenSource? _replayCts;
	public bool IsReplaying => _replayCts is not null;


	public CallerMainPageViewModel(PatternRepositoryBase repository)
	{
		_repository = repository;
		_sync = new FlashBoardSyncService(_session);
		FlashBoardVM = new FlashBoardViewModel(_session, _sync.Board);
		GameInfoVM = new GameInfoPanelViewModel(_session);
		PatternVM = new PatternDisplayViewModel(repository);

		_session.ItemCalled += (sender, item) =>
		{
			Core.FlashBoard.FlashBoardNumber? cell = _sync.Board.AllCells.FirstOrDefault(c => c.Number == item);
			if (cell != null)
			{
				Debug.WriteLine($"[ItemCalled Handler] Applying call to number {item}");
				cell.SetCalled(true, FlashBoardEventSource.Manual);
			}
		};

		GameInfoVM = new GameInfoPanelViewModel(_session);

		_ = PatternVM.LoadPatternAsync("None");

		ToggleToolsPanelCommand = new RelayCommand(() => IsToolsPanelVisible = !IsToolsPanelVisible);
		NextCallCommand = new RelayCommand(() => _session.CallNext());
		NextGameCommand = new RelayCommand(OnNextGame);

		UndoPickCommand = new RelayCommand(_session.Undo);
		RedoPickCommand = new RelayCommand(_session.Redo);
		ReplayCommand = new AsyncRelayCommand(ToggleReplayAsync);
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


	#region Replay Control
	public async Task ToggleReplayAsync()
	{
		if (_replayCts is not null)
		{
			_replayCts.Cancel();
			_replayCts = null;
			return;
		}

		await PlayReplayAsync();
	}

	public event EventHandler? ReplayStarted;
	public event EventHandler? ReplayEnded;

	public async Task PlayReplayAsync()
	{
		if (_replayCts is not null)
			return; // Already running — safeguard

		Debug.WriteLine("PlayReplayAsync() - Starting replay...");

		ReplayStarted?.Invoke(this, EventArgs.Empty);
		FlashBoardVM.SetInteractive(false); // Lock board

		Core.Domain.GameSessionSnapshot<int> originalSnapshot = _session.CreateSnapshot();
		SyncSnapshot replaySnapshot = SyncSnapshot.FromSession(_session);

		_replayCts = new CancellationTokenSource();
		CancellationToken token = _replayCts.Token;

		try
		{
			// Reset board to clean pre-replay state
			_session.Restart(replaySnapshot.CalledNumbers);

			foreach (int number in replaySnapshot.CalledNumbers)
			{
				token.ThrowIfCancellationRequested();
				_session.CallItem(number);
				await Task.Delay(3000, token); // Adjust for your pacing
			}

			// Sync final called state and rebind viewmodel wiring
			_sync.UpdateCalled(_session.CalledItems);
			FlashBoardVM.RebindModel();
		}
		catch (OperationCanceledException)
		{
			// Roll back to original state
			_session.LoadSnapshot(originalSnapshot);
			_session.SyncState(
				_sync.Board.AllCells.Where(c => c.IsCalled).Select(c => c.Number),
				_sync.Board.AllCells.Where(c => !c.IsCalled).Select(c => c.Number)
			);
			_sync.UpdateCalled(_session.CalledItems);
			FlashBoardVM.RebindModel();

			foreach (FlashBoardGroupViewModel group in FlashBoardVM.Groups)
			{
				foreach (FlashBoardCellViewModel cell in group.Cells)
				{
					if (cell.Number == 67)
						Debug.WriteLine($"PlayReplayAsync() - Cell {cell.Number} — IsCalled={cell.IsCalled} — CanToggle={cell.CanToggle}");
				}
			}
		}
		finally
		{
			FlashBoardVM.SetInteractive(true); // Unlock board
			ReplayEnded?.Invoke(this, EventArgs.Empty);
			_replayCts = null;

			Debug.WriteLine("PlayReplayAsync() - Replay ended.");
		}
	}
	#endregion

	public string ToolsPanelToggleIcon => IsToolsPanelVisible ? "collapse" : "expand";

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
