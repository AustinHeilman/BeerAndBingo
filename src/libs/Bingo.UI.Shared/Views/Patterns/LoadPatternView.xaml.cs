using Bingo.Core.Patterns;
using Bingo.ViewModel.Patterns;
using System.Diagnostics;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class LoadPatternView : ContentView
{
	private readonly PatternRepositoryBase _repository;

	public LoadPatternView(LoadPatternViewModel viewModel, PatternRepositoryBase repository)
	{
		Debug.WriteLine("[LoadPatternView] Constructor fired");
		InitializeComponent();

		_repository = repository;
		BindingContext = viewModel;

		// Manually initialize once the view is loaded and the vault is ready
		Loaded += async (_, _) =>
		{
			Debug.WriteLine("[LoadPatternView] Waiting for vault initialization...");
			await _repository.InitializeAsync();
			await viewModel.InitializeAsync();
		};
	}
}
