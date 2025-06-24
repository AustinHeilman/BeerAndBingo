using Bingo.Core.Models;

namespace Bingo.Core.Extensions;

public static class BingoPatternMatrixExtensions
{
    public static bool[,] ToMatrix(this BingoPattern pattern, int rowCount = 5, int colCount = 15)
    {
        bool[,] matrix = new bool[rowCount, colCount];
        foreach ((int row, int col) in pattern.Cells)
        {
            if (row >= 0 && row < rowCount && col >= 0 && col < colCount)
                matrix[row, col] = true;
        }
        return matrix;
    }

    public static BingoPattern FromMatrix(bool[,] matrix)
    {
        BingoPattern pattern = new();

        int rowCount = matrix.GetLength(0);
        int colCount = matrix.GetLength(1);

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < colCount; c++)
            {
                if (matrix[r, c])
                    pattern.Cells.Add((r, c));
            }
        }

        return pattern;
    }
}
