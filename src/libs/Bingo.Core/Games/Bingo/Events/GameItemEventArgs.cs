namespace Bingo.Core.Games.Bingo.Events;

public class GameItemEventArgs<T> : EventArgs
{
	public T Item { get; }
	public GameItemSource Source { get; }

	public GameItemEventArgs(T item, GameItemSource source)
	{
		Item = item;
		Source = source;
	}
}
