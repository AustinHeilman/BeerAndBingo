using Bingo.Core.Games.Bingo.BoardRules;

namespace Bingo.Core.Games.Bingo.FlashBoard;

public class FlashBoardConfig : IBingoBoardRules
{
	public int Rows => 5;
	public int Columns => 15;
	public int TotalNumbers => 75;

	public char[] GameLetters => new[] { 'B', 'I', 'N', 'G', 'O' };

	public bool IsValidNumber(int number) => number >= 1 && number <= TotalNumbers;

	public char GetLetterForNumber(int number)
	{
		if (!IsValidNumber(number))
			throw new ArgumentOutOfRangeException(nameof(number), "Number must be between 1 and 75");

		int index = (number - 1) / 15;
		return GameLetters[index];
	}

	public int GetIndexForLetter(char letter)
	{
		int index = Array.IndexOf(GameLetters, letter);
		if (index < 0)
			throw new ArgumentException($"Invalid board letter: {letter}", nameof(letter));

		return index;
	}

	public IEnumerable<int> GetNumbersInLetter(char letter)
		=> GetNumbersInIndex(GetIndexForLetter(letter));

	public IEnumerable<int> GetNumbersInIndex(int index)
	{
		if (index < 0 || index >= GameLetters.Length)
			throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 4");

		int start = index * 15 + 1;
		return Enumerable.Range(start, 15);
	}

	public int GetNumberForCell(int row, int column)
		=> column * Rows + row + 1;
}
