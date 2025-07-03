using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.ViewModel.MainPage.Caller;
using System.Windows.Input;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage
{
	public ICommand NextClockCommand { get; }
	public ICommand TogglePatternZoomCommand { get; }
	public ICommand NewGameCommand { get; }
	public ICommand UndoPickCommand { get; }
	public ICommand RedoPickCommand { get; }
	public ICommand ReplayCommand { get; }
	public ICommand QuitCommand { get; }

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

		viewModel.FlashBoardVM.SetInteractive(true);
		flashBoardView.FlashBoardVM = viewModel.FlashBoardVM;
		MainGrid.Children.Add(flashBoardView);
		Grid.SetRow(flashBoardView, 0);

		NextClockOverlay.RequestClose += (_, _) => CollapseTimerUI();

		NextClockCommand = new Command(() =>
		{
			if (!NextClockVisible)
			{
				NextClockOverlay.ViewModel.IsReadOnly = false;
				NextClockOverlay.ViewModel.SetTimer(TimeSpan.FromMinutes(10));
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

		QuitCommand = new Command(async () =>
		{
			bool confirm = await DisplayAlert("Quit App", "Do you want to quit this app?", "Yes", "No");
			if (confirm)
			{
				await QuitAppAsync();
			}
		});

		NewGameCommand = new Command(async () =>
		{
			bool confirm = await DisplayAlert("New Game", "Start a new game and reset session?", "Yes", "No");
			if (confirm)
			{
				viewModel.ResetSession();
			}
		});

		UndoPickCommand = new Command(async () =>
		{
			bool confirm = await DisplayAlert("Undo", "Undo last action?", "Yes", "No");
			if (confirm)
			{
				viewModel.UndoPickCommand.Execute(null);
			}
		});

		RedoPickCommand = new Command(async () =>
		{
			bool confirm = await DisplayAlert("New Game", "Redo last action?", "Yes", "No");
			if (confirm)
			{
				viewModel.RedoPickCommand.Execute(null);
			}
		});

		ReplayCommand = new Command(async () =>
		{
			bool confirm = await DisplayAlert("Replay", "Play all recorded game actions?\nPress replay again to stop", "Yes", "No");
			if (confirm)
			{
				viewModel.ReplayCommand.Execute(null);
			}
		});

		BindingContext = new { viewModel, page = this };
	}

	private void CollapseTimerUI()
	{
		NextClockOverlay.ViewModel.CancelTimer();
		NextClockVisible = false;
	}

	private async Task QuitAppAsync()
	{
		await Task.Delay(100); // Simulate some delay if needed
		if (OperatingSystem.IsWindows())
		{
			// Windows-specific quit logic
			Application.Current?.Quit();
		}
#if ANDROID
		else if (OperatingSystem.IsAndroid())
		{
			// Android-specific quit logic			
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}
#elif IOS
		else if (OperatingSystem.IsIOS())
		{
			// iOS-specific quit logic
			await DisplayAlert("Unsupported", "Please press the Home button to exit the app.", "OK");
		}
#endif
		else if (OperatingSystem.IsMacOS())
		{
			// macOS-specific quit logic
			Application.Current?.Quit();
		}
		else if (OperatingSystem.IsLinux())
		{
			// Linux-specific quit logic
			Application.Current?.Quit();
		}
		else
		{
			// For other platforms, use the platform-specific APIs
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}
	}
}
