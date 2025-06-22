namespace Bingo.Services.Patterns;

public static class PatternRotationService
{
    public static ISet<(int row, int col)> Rotate90(ISet<(int row, int col)> pattern, int rowCount = 5, int colCount = 15)
    {
        return pattern.Select(cell => (row: cell.col, col: rowCount - 1 - cell.row)).ToHashSet();
    }

    public static ISet<(int row, int col)> Rotate180(ISet<(int row, int col)> pattern, int rowCount = 5, int colCount = 15)
    {
        return pattern.Select(cell => (row: rowCount - 1 - cell.row, col: colCount - 1 - cell.col)).ToHashSet();
    }

    public static ISet<(int row, int col)> Rotate270(ISet<(int row, int col)> pattern, int rowCount = 5, int colCount = 15)
    {
        return pattern.Select(cell => (row: colCount - 1 - cell.col, col: cell.row)).ToHashSet();
    }
}

