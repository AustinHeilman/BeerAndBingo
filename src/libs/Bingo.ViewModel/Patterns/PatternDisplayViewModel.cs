using Bingo.Core.Patterns;
using Bingo.Services.Patterns;
using System.Collections.ObjectModel;

namespace Bingo.ViewModel.Patterns;

public class PatternDisplayViewModel
{
	private readonly IPatternRepository _repository;

	public ObservableCollection<PatternCell> PatternCells { get; } = new();

	public PatternDisplayViewModel(IPatternRepository repository)
	{
		_repository = repository;

		// Optional: preload grid with fully empty default to show gray grid
		LoadFromPattern(BingoPattern.EmptyPattern);
	}

	public async Task LoadPatternAsync(string name)
	{
		BingoPattern? pattern = await _repository.GetByNameAsync(name)
								   ?? BingoPattern.EmptyPattern;

		LoadFromPattern(pattern);
	}

	public void LoadFromPattern(BingoPattern pattern)
	{
		PatternCells.Clear();

		foreach (PatternCell cell in pattern.Cells)
		{
			PatternCells.Add(new PatternCell
			{
				Row = cell.Row,
				Col = cell.Col,
				IsActive = cell.IsActive
			});
		}
	}
}
