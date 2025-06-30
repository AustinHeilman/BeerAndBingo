namespace Bingo.Core.Games.Bingo.BoardRules;

public interface IBingoBoardRules
{
	int Rows { get; }
	int Columns { get; }
	int TotalNumbers { get; }

	char[] GameLetters { get; }

	bool IsValidNumber(int number);
	char GetLetterForNumber(int number);
	int GetIndexForLetter(char letter);
	IEnumerable<int> GetNumbersInLetter(char letter);
	IEnumerable<int> GetNumbersInIndex(int index);
	int GetNumberForCell(int row, int column);
}
