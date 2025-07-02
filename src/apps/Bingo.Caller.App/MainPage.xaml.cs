using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.ViewModel.MainPage.Caller;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	public ICommand NewGameCommand { get; }

	public MainPage(FlashBoardView flashBoardView, CallerMainPageViewModel viewModel)
	{
		InitializeComponent();

		flashBoardView.IsInteractive = true;
		flashBoardView.ViewModel = viewModel.FlashBoardVM;
		MainGrid.Children.Add(flashBoardView);
		Grid.SetRow(flashBoardView, 0);

		NewGameCommand = new Command(async () =>
		{
			bool confirmed = await DisplayAlert(
				"Start New Game?",
				"This will reset the board and call history. Are you sure?",
				"Yes", "No");

			if (confirmed)
				viewModel.ResetSession();
		});

		BindingContext = new { viewModel, page = this };
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
