using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.ViewModel.MainPage.Caller;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage
{
	public MainPage(FlashBoardView flashBoardView, CallerMainPageViewModel viewModel)
	{
		InitializeComponent();

		// Set up the FlashBoardView        
		flashBoardView.IsInteractive = true;
		flashBoardView.VerticalOptions = LayoutOptions.Fill;
		flashBoardView.HorizontalOptions = LayoutOptions.Fill;
		flashBoardView.ViewModel = viewModel.FlashBoardVM;

		// Add FlashBoardView to the MainGrid at row 0
		MainGrid.Children.Add(flashBoardView);
		Grid.SetRow(flashBoardView, 0);

		BindingContext = viewModel;
	}
}
