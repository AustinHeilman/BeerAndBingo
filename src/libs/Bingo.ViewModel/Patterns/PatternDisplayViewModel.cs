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
    }

    public async Task LoadPatternAsync(string name)
    {
        var pattern = await _repository.GetByNameAsync(name);
        if (pattern is null)
            return;

        PatternCells.Clear();

        foreach (var (row, col) in pattern.Cells)
        {
            PatternCells.Add(new PatternCell
            {
                Row = row,
                Col = col,
                IsActive = true
            });
        }
    }
}
