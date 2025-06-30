namespace Bingo.Core.Games.Bingo.State.Events;

public class GameUndoEventArgs : EventArgs
{
	public int RemovedNumber { get; }
	public int? RestoredPrevious { get; }

	public GameUndoEventArgs(int removedNumber, int? restoredPrevious)
	{
		RemovedNumber = removedNumber;
		RestoredPrevious = restoredPrevious;
	}
}
