namespace Bingo.Core.Games.Bingo.Patterns.Extensions;

public static class BingoPatternMatrixExtensions
{
	public static bool[,] ToMatrix(this BingoPattern pattern)
	{
		int rows = PatternGridSettings.PatternRowCount;
		int cols = PatternGridSettings.PatternColCount;
		bool[,] matrix = new bool[rows, cols];

		foreach (PatternCell cell in pattern.Cells)
		{
			matrix[cell.Row, cell.Col] = cell.IsActive;
		}

		return matrix;
	}

	public static BingoPattern FromMatrix(bool[,] matrix)
	{
		BingoPattern result = new();

		for (int r = 0; r < matrix.GetLength(0); r++)
		{
			for (int c = 0; c < matrix.GetLength(1); c++)
			{
				result.Cells.Add(new PatternCell
				{
					Row = r,
					Col = c,
					IsActive = matrix[r, c]
				});
			}
		}

		return result;
	}
}
