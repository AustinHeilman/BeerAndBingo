using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.ViewModel.MainPage.Caller;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage
{
	public MainPage(FlashBoardView flashBoardView, CallerMainPageViewModel viewModel)
	{
		InitializeComponent();

		// Inject FlashBoardView
		flashBoardView.IsInteractive = true;
		flashBoardView.VerticalOptions = LayoutOptions.Fill;
		flashBoardView.HorizontalOptions = LayoutOptions.Fill;
		flashBoardView.ViewModel = viewModel.FlashBoardVM;

		MainGrid.Children.Add(flashBoardView);
		Grid.SetRow(flashBoardView, 0);
		Grid.SetColumnSpan(flashBoardView, 4);

		BindingContext = viewModel;
	}
}
