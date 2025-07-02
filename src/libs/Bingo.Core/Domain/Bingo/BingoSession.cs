namespace Bingo.Core.Domain.Bingo;

public class BingoSession : GameSessionState<int>
{
	public const int Min = 1;
	public const int Max = 75;

	public BingoSession() : base(Enumerable.Range(Min, Max)) { }

	public static string FormatCall(int number) => $"{GetLetter(number)}-{number}";

	public static char GetLetter(int number)
	{
		if (number < 1 || number > Max)
			throw new ArgumentOutOfRangeException(nameof(number));

		return (number - 1) switch
		{
			< 15 => 'B',
			< 30 => 'I',
			< 45 => 'N',
			< 60 => 'G',
			_ => 'O'
		};
	}
}
