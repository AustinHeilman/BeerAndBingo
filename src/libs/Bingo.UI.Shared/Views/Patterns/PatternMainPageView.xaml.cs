using Bingo.ViewModel.Patterns;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class PatternMainPageView : ContentView
{
	public PatternMainPageView(PatternMainPageViewModel viewModel)
	{
		InitializeComponent();
		self.BindingContext = viewModel;
	}
}