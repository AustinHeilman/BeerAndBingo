using System.Diagnostics;

namespace Bingo.Core.Domain;

public class GameSessionState<T>
{
	#region Fields

	private readonly List<T> _history = new();
	private int _pointer = -1;

	private readonly HashSet<T> _calledSet = new();
	private readonly HashSet<T> _availableSet = new();

	private DateTime _startedAt = DateTime.UtcNow;

	#endregion

	#region Events

	public event EventHandler<T>? ItemCalled;
	public event EventHandler<(T undoneItem, T? newCurrent)>? UndoPerformed;
	public event EventHandler<T>? RedoPerformed;
	public event EventHandler? NewGameStarted;

	#endregion

	#region Properties

	public T? CurrentItem => _pointer >= 0 && _pointer < _history.Count ? _history[_pointer] : default;
	public T? PreviousItem => _pointer > 0 ? _history[_pointer - 1] : default;

	public IReadOnlyList<T> CalledItems => _history.Take(_pointer + 1).ToList();
	public IReadOnlyList<T> AvailableItems => _availableSet.ToList();

	public int Round => _pointer + 1;
	public bool IsDone => _availableSet.Count == 0;

	public string SessionId { get; set; } = Guid.NewGuid().ToString();

	#endregion

	#region Constructor

	public GameSessionState(IEnumerable<T> fullSet)
	{
		Restart(fullSet);
	}

	#endregion

	#region Core API

	public void CallNext()
	{
		if (IsDone) return;

		T next = _availableSet.First(); // Will throw if empty, but IsDone guards against that
		CallItem(next);
	}


	public void CallItem(T item)
	{
		Debug.WriteLine($"[GameSession] CallItem invoked with {item}");
		if (_calledSet.Contains(item) || !_availableSet.Contains(item))
			return;

		if (_pointer < _history.Count - 1)
			_history.RemoveRange(_pointer + 1, _history.Count - (_pointer + 1));

		_history.Add(item);
		_pointer++;

		_calledSet.Add(item);
		_availableSet.Remove(item);

		ItemCalled?.Invoke(this, item);
	}

	public void Undo()
	{
		if (_pointer < 0) return;

		var undone = _history[_pointer];
		_pointer--;

		_calledSet.Remove(undone);
		_availableSet.Add(undone);

		UndoPerformed?.Invoke(this, (undone, CurrentItem));
	}

	public void Redo()
	{
		if (_pointer >= _history.Count - 1) return;

		_pointer++;
		var redone = _history[_pointer];

		_calledSet.Add(redone);
		_availableSet.Remove(redone);

		RedoPerformed?.Invoke(this, redone);
	}

	public void Restart(IEnumerable<T> newSet)
	{
		_history.Clear();
		_pointer = -1;
		_calledSet.Clear();
		_availableSet.Clear();

		var shuffled = newSet.ToList();
		Shuffle(shuffled);

		foreach (var item in shuffled)
			_availableSet.Add(item);

		_startedAt = DateTime.UtcNow;
		SessionId = Guid.NewGuid().ToString();

		NewGameStarted?.Invoke(this, EventArgs.Empty);
	}

	#endregion

	#region Snapshot

	public GameSessionSnapshot<T> CreateSnapshot() => new(
		History: _history.ToList(),
		Pointer: _pointer,
		StartedAt: _startedAt,
		SessionId: SessionId
	);

	public void LoadSnapshot(GameSessionSnapshot<T> snapshot)
	{
		_history.Clear();
		_history.AddRange(snapshot.History);

		_pointer = snapshot.Pointer;

		_calledSet.Clear();
		_availableSet.Clear();

		foreach (var item in _history.Take(_pointer + 1))
			_calledSet.Add(item);

		foreach (var item in _history.Skip(_pointer + 1))
			_availableSet.Add(item);

		_startedAt = snapshot.StartedAt;
		SessionId = snapshot.SessionId ?? Guid.NewGuid().ToString();

		if (CurrentItem is not null)
			ItemCalled?.Invoke(this, CurrentItem);
	}

	#endregion

	#region Helpers

	private void Shuffle(List<T> list)
	{
		var rng = new Random();
		for (int i = list.Count - 1; i > 0; i--)
		{
			int j = rng.Next(i + 1);
			(list[i], list[j]) = (list[j], list[i]);
		}
	}

	#endregion
}
