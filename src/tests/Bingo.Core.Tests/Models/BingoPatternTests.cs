using Bingo.Core.Games.Bingo.Patterns;
using Bingo.Services.Patterns;

namespace Bingo.AppServices.Patterns;

public class DefaultPatternRepository : IPatternRepository
{
	private readonly List<BingoPattern> _patterns = new()
	{
		new BingoPattern
		{
			Name = "4 Corners",
			Cells = new HashSet<PatternCell>(
				new[]
				{
					(0, 0), (0, 4),
					(4, 0), (4, 4)
				}.Select(pos => new PatternCell
				{
					Row = pos.Item1,
					Col = pos.Item2,
					IsActive = true
				}))
		},

		new BingoPattern
		{
			Name = "X Pattern",
			Cells = new HashSet<PatternCell>(
				Enumerable.Range(0, 5)
					.SelectMany(i => new[] { (i, i), (i, 4 - i) })
					.Select(pos => new PatternCell
					{
						Row = pos.Item1,
						Col = pos.Item2,
						IsActive = true
					}))
		}
	};

	public Task<BingoPattern?> GetByNameAsync(string name)
	{
		BingoPattern? pattern = _patterns.FirstOrDefault(p => p.Name == name);
		return Task.FromResult(pattern);
	}

	public Task<IEnumerable<BingoPattern>> GetAllAsync()
	{
		return Task.FromResult<IEnumerable<BingoPattern>>(_patterns);
	}

	public Task SaveAsync(BingoPattern pattern)
	{
		BingoPattern? existing = _patterns.FirstOrDefault(p => p.Name == pattern.Name);
		if (existing is not null)
			_patterns.Remove(existing);

		_patterns.Add(pattern);
		return Task.CompletedTask;
	}

	public Task DeleteAsync(string name)
	{
		BingoPattern? match = _patterns.FirstOrDefault(p => p.Name == name);
		if (match is not null)
			_patterns.Remove(match);

		return Task.CompletedTask;
	}
}
