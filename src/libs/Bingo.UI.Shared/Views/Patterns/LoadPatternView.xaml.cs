using Bingo.Core.Patterns;
using Bingo.ViewModel.Patterns;
using Microsoft.Maui.Controls;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class LoadPatternView : ContentView
{
	public LoadPatternView(LoadPatternViewModel viewModel, PatternRepositoryBase repository)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
