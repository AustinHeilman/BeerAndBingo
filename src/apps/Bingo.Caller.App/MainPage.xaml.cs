using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.ViewModel.MainPage.Caller;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	public ICommand NextClockCommand { get; }

	private bool _nextClockVisible = false;
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

	public MainPage(FlashBoardView flashBoardView, CallerMainPageViewModel viewModel)
	{
		InitializeComponent();

		flashBoardView.IsInteractive = true;
		flashBoardView.ViewModel = viewModel.FlashBoardVM;
		MainGrid.Children.Add(flashBoardView);
		Grid.SetRow(flashBoardView, 0);

		// Listen for safe closure event from timer overlay
		NextClockOverlay.RequestClose += (_, _) => CollapseTimerUI();

		NextClockCommand = new Command(() =>
		{
			if (!NextClockVisible)
			{
				NextClockOverlay.ViewModel.IsReadOnly = false;
				NextClockOverlay.ViewModel.SetTimer(TimeSpan.FromMinutes(5));
				NextClockVisible = true;
			}
			else
			{
				CollapseTimerUI();
			}
		});

		BindingContext = new { viewModel, page = this };
	}

	private void CollapseTimerUI()
	{
		NextClockOverlay.ViewModel.CancelTimer();
		NextClockVisible = false;
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string name = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
