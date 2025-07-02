using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.ViewModel.MainPage.Caller;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage
{
	public ICommand NextClockCommand { get; }

	public bool NextClockVisible
	{
		get => _nextClockVisible;
		set
		{
			if (_nextClockVisible != value)
			{
				_nextClockVisible = value;
				OnPropertyChanged(nameof(NextClockVisible));
			}
		}
	}
	private bool _nextClockVisible = false;

	public MainPage(FlashBoardView flashBoardView, CallerMainPageViewModel viewModel)
	{
		InitializeComponent();

		flashBoardView.IsInteractive = true;
		flashBoardView.ViewModel = viewModel.FlashBoardVM;
		MainGrid.Children.Add(flashBoardView);
		Grid.SetRow(flashBoardView, 0);

		NextClockCommand = new Command(() =>
		{
			if (!NextClockVisible)
			{
				NextClockOverlay.ViewModel.IsReadOnly = false;
				NextClockOverlay.ViewModel.SetTimer(TimeSpan.FromMinutes(5)); // Optional default
			}
			else
			{
				NextClockOverlay.ViewModel.CancelTimer();
			}

			NextClockVisible = !NextClockVisible;
		});

		BindingContext = new { viewModel, page = this };
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string name = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
