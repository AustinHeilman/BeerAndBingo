using Bingo.Core.Patterns;

namespace Bingo.Core.Extensions;

public static class BingoPatternMatrixExtensions
{
	public static bool[,] ToMatrix(this BingoPattern pattern, int rowCount, int colCount)
	{
		bool[,] matrix = new bool[rowCount, colCount];
		foreach ((int row, int col) in pattern.Cells)
		{
			if (row >= 0 && row < rowCount && col >= 0 && col < colCount)
				matrix[row, col] = true;
		}
		return matrix;
	}

	public static bool[,] ToMatrix(this BingoPattern pattern)
	{
		return pattern.ToMatrix(PatternGridSettings.PatternRowCount, PatternGridSettings.PatternColCount);
	}

	public static BingoPattern FromMatrix(bool[,] matrix)
	{
		BingoPattern pattern = new();

		int rowCount = PatternGridSettings.PatternRowCount;
		int colCount = PatternGridSettings.PatternColCount;

		for (int r = 0; r < rowCount; r++)
		{
			for (int c = 0; c < colCount; c++)
			{
				// Ensure we don't go out of bounds if the matrix is smaller
				if (r < matrix.GetLength(0) && c < matrix.GetLength(1) && matrix[r, c])
					pattern.Cells.Add((r, c));
			}
		}

		return pattern;
	}
}
