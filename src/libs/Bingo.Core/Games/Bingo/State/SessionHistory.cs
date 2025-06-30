namespace Bingo.Core.Games.Bingo.State;

public class SessionHistory<T>
{
	private readonly Stack<T> callStack = new();
	private readonly Stack<T> redoStack = new();

	public int Count => callStack.Count;
	public T? Current => callStack.TryPeek(out T? c) ? c : default;
	public T? Previous => callStack.Skip(1).FirstOrDefault();

	public bool Contains(T item) => callStack.Contains(item);

	public void RecordCall(T item)
	{
		callStack.Push(item);
		redoStack.Clear();
	}

	public bool TryUndo(out T removed)
	{
		if (callStack.TryPop(out removed))
		{
			redoStack.Push(removed);
			return true;
		}

		return false;
	}

	public bool TryRedo(out T restored)
	{
		if (redoStack.TryPop(out restored))
		{
			callStack.Push(restored);
			return true;
		}

		return false;
	}

	public void Clear()
	{
		callStack.Clear();
		redoStack.Clear();
	}

	public IReadOnlyList<T> CallSequence => callStack.Reverse().ToList();
}
