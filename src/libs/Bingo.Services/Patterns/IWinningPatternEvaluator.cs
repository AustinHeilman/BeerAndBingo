using Bingo.Core.Models;

namespace Bingo.Services.Patterns;

public interface IWinningPatternEvaluator
{
    bool IsWinning(BingoPattern pattern, IReadOnlySet<(int row, int col)> markedCells);
}
