using Bingo.Core.Patterns;

public interface IPatternService
{
	Task<IEnumerable<string>> GetPatternNamesAsync();
	Task<BingoPattern?> GetByNameAsync(string name);
	Task SaveAsync(BingoPattern pattern);
	Task DeleteAsync(string name);
}
