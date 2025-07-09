using Bingo.ViewModel.Patterns;
using System.Diagnostics;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class LoadPatternView : ContentView
{
	public LoadPatternView(LoadPatternViewModel viewModel)
	{
		Debug.WriteLine("[LoadPatternView] Received ViewModel instance");
		InitializeComponent();
		BindingContext = viewModel;
	}
}
