using Bingo.Core.Patterns;
using Bingo.ViewModel.Messages.Patterns;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Bingo.ViewModel.Patterns;

public partial class LoadPatternViewModel : ObservableObject
{
	private readonly PatternRepositoryBase _repository;

	public ObservableCollection<BingoPattern> SavedPatterns { get; } = new();

	[ObservableProperty]
	private BingoPattern? selectedPattern;

	public LoadPatternViewModel(PatternRepositoryBase repository)
	{
		_repository = repository;
		Debug.WriteLine("[LoadPatternViewModel] Constructor entered");
	}

	public async Task InitializeAsync()
	{
		SavedPatterns.Clear();		
		var patterns = await _repository.GetAllPatternsAsync();
		patterns = patterns.OrderBy(p => p.Name).ToImmutableList();

		Debug.WriteLine($"[LoadPatternViewModel] Loaded {patterns.Count()} patterns");

		foreach (var pattern in patterns)
		{
			Debug.WriteLine($"[LoadPatternViewModel] Pattern: {pattern.Name}");
			SavedPatterns.Add(pattern);
		}
	}

	[RelayCommand]
	private void ConfirmLoad()
	{
		if (SelectedPattern is null) return;

		Debug.WriteLine($"[LoadPatternViewModel] Confirming load of pattern: {SelectedPattern.Name}");
		_repository.SetActivePattern(SelectedPattern);
		WeakReferenceMessenger.Default.Send(new CloseLoadPatternMessage());
	}

	[RelayCommand]
	private void Close()
	{
		WeakReferenceMessenger.Default.Send(new CloseLoadPatternMessage());
	}
}
