using Bingo.Core.Patterns;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class PatternMainPageView : ContentView
{
	private readonly PatternRepositoryBase _repository;

	public ICommand ClearCommand { get; }

	public PatternMainPageView(PatternRepositoryBase repository)
	{
		InitializeComponent();
		_repository = repository;

		ClearCommand = new Command(() => {
			_repository.SetActivePattern(BingoPattern.EmptyPattern);			
		});

		BindingContext = this;
	}

	public ICommand? GetClearCommand()
	{
		// Assumes BindingContext is set to a ViewModel with ClearCommand
		_repository.SetActivePattern(BingoPattern.EmptyPattern);
		return (BindingContext as dynamic)?.ClearCommand;
	}
}