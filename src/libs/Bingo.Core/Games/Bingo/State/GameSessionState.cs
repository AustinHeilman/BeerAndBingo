using Bingo.Core.Games.Bingo.Events;

namespace Bingo.Core.Games.Bingo.State;

public class GameSessionState<T>
{
	private readonly SessionHistory<T> history = new();
	private readonly ReplayController<T> replay = new();

	public int Round => history.Count;
	public T? CurrentItem => history.Current;
	public T? PreviousItem => history.Previous;
	public bool IsReplayMode => replay.IsActive;

	public event EventHandler? ReplayStarted;
	public event EventHandler? ReplayEnded;
	public event EventHandler<GameUndoEventArgs<T>>? UndoPerformed;
	public event EventHandler? RedoPerformed;
	public event EventHandler<GameItemEventArgs<T>>? ItemCalled;

	public void Call(T item, GameItemSource source = GameItemSource.Manual)
	{
		if (history.Contains(item))
			return;

		history.RecordCall(item);
		ItemCalled?.Invoke(this, new GameItemEventArgs<T>(item, source));
	}

	public void Undo()
	{
		if (history.TryUndo(out T? removed))
		{
			T? restored = history.Current;
			UndoPerformed?.Invoke(this, new GameUndoEventArgs<T>(removed, restored));
			ItemCalled?.Invoke(this, new GameItemEventArgs<T>(removed, GameItemSource.Undo));
		}
	}

	public void Redo()
	{
		if (history.TryRedo(out T? item))
		{
			RedoPerformed?.Invoke(this, EventArgs.Empty);
			ItemCalled?.Invoke(this, new GameItemEventArgs<T>(item, GameItemSource.Redo));
		}
	}

	public void NewGame()
	{
		history.Clear();
		replay.Cancel();
	}

	public void BeginReplay(IEnumerable<T> sequence, int delayMs = 1000, CancellationToken? token = null)
	{
		replay.Start(sequence, delayMs, token);
		ReplayStarted?.Invoke(this, EventArgs.Empty);

		replay.ReplayStep += OnReplayStep;
		replay.ReplayCompleted += (_, _) =>
		{
			ReplayEnded?.Invoke(this, EventArgs.Empty);
			replay.ReplayStep -= OnReplayStep;
		};
	}

	public void CancelReplay()
	{
		replay.Cancel();
		ReplayEnded?.Invoke(this, EventArgs.Empty);
	}

	private void OnReplayStep(T item)
	{
		history.RecordCall(item);
		ItemCalled?.Invoke(this, new GameItemEventArgs<T>(item, GameItemSource.Replay));
	}
}
