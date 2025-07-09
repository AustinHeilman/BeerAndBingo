using Bingo.ViewModel.Patterns;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class LoadPatternView : ContentView
{
	public LoadPatternView(LoadPatternViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
