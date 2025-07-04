using Bingo.Core.Patterns;
using Bingo.Services.Patterns.Events;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace Bingo.ViewModel.Patterns;

public class PatternDisplayViewModel
{
	private readonly PatternRepositoryBase _repository;

	public ObservableCollection<PatternCell> PatternCells { get; } = new();

	public PatternDisplayViewModel(PatternRepositoryBase repository)
	{
		_repository = repository;
		LoadFromPattern(BingoPattern.EmptyPattern);

		WeakReferenceMessenger.Default.Register<PatternChangedEvent>(this, (r, m) =>
		{
			LoadFromPattern(m.Pattern); // your viewmodel method
		});
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
