using Bingo.Core.Models;

namespace Bingo.Core.Extensions
{
    public static class IsSymmetricalExtension
    {
        public static bool IsSymmetrical(this BingoPattern pattern)
        {
            return pattern.Cells.All(cell =>
                pattern.Cells.Contains((cell.Row, 14 - cell.Col))
            );
        }
    }
}
