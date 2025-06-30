namespace Bingo.Core.Games.Bingo.State.Events;

public class GameNumberEventArgs : EventArgs
{
	public int Number { get; }
	public GameNumberSource Source { get; }

	public GameNumberEventArgs(int number, GameNumberSource source)
	{
		Number = number;
		Source = source;
	}
}
