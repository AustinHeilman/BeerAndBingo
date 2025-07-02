namespace Bingo.Core.Domain.Bingo;

public class SyncSnapshot
{
	public List<int> CalledNumbers { get; init; } = new();
	public int CurrentRound => CalledNumbers.Count;

	public static SyncSnapshot FromSession(GameSessionState<int> session)
		=> new() { CalledNumbers = session.CalledItems.ToList() };
}
