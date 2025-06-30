using Bingo.Core.Games.Bingo.Patterns;
using Bingo.Services.Patterns;

namespace Bingo.AppServices.Patterns;

public class PatternService : IPatternService
{
	private readonly IPatternRepository _repository;

	public PatternService(IPatternRepository repository)
	{
		_repository = repository;
	}

	public async Task<IEnumerable<string>> GetPatternNamesAsync()
	{
		IEnumerable<BingoPattern> patterns = await _repository.GetAllAsync();
		return patterns.Select(p => p.Name);
	}

	public async Task<BingoPattern?> GetByNameAsync(string name)
	{
		return await _repository.GetByNameAsync(name);
	}

	public async Task SaveAsync(BingoPattern pattern)
	{
		await _repository.SaveAsync(pattern);
	}

	public async Task DeleteAsync(string name)
	{
		await _repository.DeleteAsync(name);
	}
}
