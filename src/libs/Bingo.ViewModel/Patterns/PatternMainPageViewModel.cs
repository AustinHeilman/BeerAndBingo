using Bingo.Core.Patterns;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Bingo.ViewModel.Messages.Patterns;

namespace Bingo.ViewModel.Patterns;

public partial class PatternMainPageViewModel : ObservableObject
{
	private readonly PatternRepositoryBase _repository;


	public PatternMainPageViewModel(PatternRepositoryBase repository)
	{
		_repository = repository;
	}

	[RelayCommand]
	private void CreatePattern()
	{
		WeakReferenceMessenger.Default.Send(new OpenCreatePatternMessage());
	}

	[RelayCommand]
	private void LoadPattern()
	{
		WeakReferenceMessenger.Default.Send(new OpenLoadPatternMessage());
	}

	[RelayCommand]
	private void ClearPattern()
	{
		_repository.SetActivePattern(BingoPattern.EmptyPattern);
	}

	[RelayCommand]
	private void CloseSheet()
	{
		WeakReferenceMessenger.Default.Send(new ClosePatternSheetMessage());
	}
}
