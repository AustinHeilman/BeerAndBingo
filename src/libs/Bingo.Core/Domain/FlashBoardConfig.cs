namespace Bingo.Core.Domain;

/// <summary>
/// Provides configuration and helper methods for a standard Bingo board.
/// </summary>
public static class FlashBoardConfig
{
    public const int Rows = 5;
    public const int Columns = 15;
    public const int TotalNumbers = 75;

    /// <summary>
    /// The standard letters used in Bingo (B, I, N, G, O).
    /// </summary>
    public static readonly char[] GameLetters = { 'B', 'I', 'N', 'G', 'O' };

    /// <summary>
    /// Calculates the Bingo number at a specific 0-based (row, column) coordinate on a column-major grid.
    /// </summary>
    public static int GetNumberForCell(int row, int column)
    {
        return (column * Rows) + row + 1;
    }

    /// <summary>
    /// Returns the index (0–4) of a given board letter (B, I, N, G, O).
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the letter is not a valid Bingo header.</exception>
    public static int GetIndexForBoardLetter(char letter)
    {
        int index = Array.IndexOf(GameLetters, letter);
        if (index < 0)
            throw new ArgumentException($"Invalid board letter: {letter}", nameof(letter));

        return index;
    }

    /// <summary>
    /// Returns the 15-number range associated with a board column index (0–4).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if index is outside 0 to 4.</exception>
    public static IEnumerable<int> GetNumbersInBoardIndex(int index)
    {
        if (index < 0 || index >= GameLetters.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 4 for standard BINGO");

        int start = index * 15 + 1;
        return Enumerable.Range(start, 15);
    }

    /// <summary>
    /// Returns the 15-number range associated with a board letter (B, I, N, G, O).
    /// </summary>
    public static IEnumerable<int> GetNumbersInBoardLetter(char letter)
    {
        int index = GetIndexForBoardLetter(letter);
        return GetNumbersInBoardIndex(index);
    }

    /// <summary>
    /// Returns the corresponding letter (B, I, N, G, O) for a given number between 1 and 75.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if number is outside valid range.</exception>
    public static char GetLetterForNumber(int number)
    {
        if (!IsValidNumber(number))
            throw new ArgumentOutOfRangeException(nameof(number), "Number must be between 1 and 75");

        int index = (number - 1) / 15;
        return GameLetters[index];
    }

    /// <summary>
    /// Determines whether a given number is valid for a standard Bingo board.
    /// </summary>
    public static bool IsValidNumber(int number)
    {
        return number >= 1 && number <= TotalNumbers;
    }
}
