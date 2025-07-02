using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.ViewModel.MainPage.Caller;
using System.Windows.Input;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage
{
	public ICommand NextClockCommand { get; }
	public ICommand TogglePatternZoomCommand { get; }

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

	private bool _isPatternZoomed = false;
	public bool IsPatternZoomed
	{
		get => _isPatternZoomed;
		set
		{
			if (_isPatternZoomed != value)
			{
				_isPatternZoomed = value;
				OnPropertyChanged(nameof(IsPatternZoomed));
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

		TogglePatternZoomCommand = new Command(() =>
		{
			IsPatternZoomed = !IsPatternZoomed;
		});

		BindingContext = new { viewModel, page = this };
	}

	private void CollapseTimerUI()
	{
		NextClockOverlay.ViewModel.CancelTimer();
		NextClockVisible = false;
	}
}
