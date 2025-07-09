using Bingo.Core.Patterns;
using Bingo.ViewModel.Messages.Patterns;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

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
		LoadCachedPatterns();
	}

	private void LoadCachedPatterns()
	{
		SavedPatterns.Clear();
		var patterns = _repository.GetAllPatternsAsync().GetAwaiter().GetResult();
		foreach (var pattern in patterns)
			SavedPatterns.Add(pattern);
	}

	[RelayCommand]
	private void ConfirmLoad()
	{
		if (SelectedPattern is null) return;

		_repository.SetActivePattern(SelectedPattern);
		WeakReferenceMessenger.Default.Send(new CloseLoadPatternMessage());
	}

	[RelayCommand]
	private void Close()
	{
		WeakReferenceMessenger.Default.Send(new CloseLoadPatternMessage());
	}
}
