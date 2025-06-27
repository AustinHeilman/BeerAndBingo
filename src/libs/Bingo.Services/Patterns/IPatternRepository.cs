using Bingo.Core.Patterns;

namespace Bingo.Services.Patterns;

public interface IPatternRepository
{
	Task<IEnumerable<BingoPattern>> GetAllAsync();
	Task<BingoPattern?> GetByNameAsync(string name);
	Task SaveAsync(BingoPattern pattern);
	Task DeleteAsync(string name);
}
