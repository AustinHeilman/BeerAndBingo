using Bingo.Core.Patterns;
using Bingo.ViewModel.Patterns;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class PatternMainPageView : ContentView
{
	public PatternMainPageView(PatternMainPageViewModel viewModel)
	{
		InitializeComponent();
		self.BindingContext = viewModel;
	}
}