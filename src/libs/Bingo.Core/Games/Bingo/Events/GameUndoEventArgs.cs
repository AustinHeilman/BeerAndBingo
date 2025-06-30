namespace Bingo.Core.Games.Bingo.Events;

public class GameUndoEventArgs<T> : EventArgs
{
	public T RemovedItem { get; }
	public T? RestoredPrevious { get; }

	public GameUndoEventArgs(T removedItem, T? restoredPrevious)
	{
		RemovedItem = removedItem;
		RestoredPrevious = restoredPrevious;
	}
}
