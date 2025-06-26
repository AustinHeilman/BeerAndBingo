using Bingo.Core.Patterns;

namespace Bingo.Core.Extensions
{
    public static class IsSymmetricalExtension
    {
        public enum SymmetryKind { Horizontal, Vertical, Diagonal }

        public static bool IsSymmetrical(this BingoPattern pattern, SymmetryKind kind = SymmetryKind.Horizontal)
        {
            int maxRow = PatternGridSettings.PatternRowCount - 1;
            int maxCol = PatternGridSettings.PatternColCount - 1;

            return kind switch
            {
                SymmetryKind.Horizontal =>
                    pattern.Cells.All(cell =>
                        pattern.Cells.Contains((cell.Row, maxCol - cell.Col))
                    ),
                SymmetryKind.Vertical =>
                    pattern.Cells.All(cell =>
                        pattern.Cells.Contains((maxRow - cell.Row, cell.Col))
                    ),
                SymmetryKind.Diagonal =>
                    pattern.Cells.All(cell =>
                        pattern.Cells.Contains((cell.Col, cell.Row))
                    ),
                _ => false
            };
        }
    }
}
