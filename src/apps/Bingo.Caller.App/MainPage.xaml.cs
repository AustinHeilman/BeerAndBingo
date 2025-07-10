using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.UI.Shared.Views.Patterns;
using Bingo.ViewModel.MainPage;
using Bingo.ViewModel.Messages.Patterns;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Windows.Input;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	public ICommand NextClockCommand { get; }
	public ICommand TogglePatternZoomCommand { get; }
	public ICommand NewGameCommand { get; }
	public ICommand UndoPickCommand { get; }
	public ICommand RedoPickCommand { get; }
	public ICommand ReplayCommand { get; }
	public ICommand QuitCommand { get; }

	private bool _nextClockVisible = false;
	private readonly CreatePatternView _createPatternView;
	private readonly LoadPatternView _loadPatternView;
	private readonly PatternMainPageView _patternMainpageView;
	private readonly CallerMainPageViewModel _viewModel;

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

	public MainPage(FlashBoardView flashBoardView, CallerMainPageViewModel viewModel,
		CreatePatternView createPatternView, PatternMainPageView patternMainpageView, LoadPatternView loadPatternView)
	{
		InitializeComponent();

		viewModel.FlashBoardVM.SetInteractive(true);
		flashBoardView.FlashBoardVM = viewModel.FlashBoardVM;
		MainGrid.Children.Add(flashBoardView);
		Grid.SetRow(flashBoardView, 0);
		_createPatternView = createPatternView;
		_loadPatternView = loadPatternView;
		_patternMainpageView = patternMainpageView;
		_viewModel = viewModel;

		NextClockOverlay.RequestClose += (_, _) => CollapseTimerUI();

		NextClockCommand = new Command(() =>
		{
			if (!NextClockVisible)
			{
				NextClockOverlay.ViewModel.IsReadOnly = false;
				NextClockOverlay.ViewModel.SetTimer(TimeSpan.FromMinutes(10));
				NextClockVisible = true;
				_viewModel.SetFlightBarExpanded(false);
			}
			else
			{
				CollapseTimerUI();
			}
		});

		WeakReferenceMessenger.Default.Register<CloseCreatePatternMessage>(this, (r, m) =>
		{
			HideCreatePatternCommand.Execute(null);
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
				_viewModel.SetFlightBarExpanded(false);

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

		WeakReferenceMessenger.Default.Register<ClosePatternSheetMessage>(this, (r, m) =>
		{
			HidePatternSheetCommand.Execute(null);
		});
		WeakReferenceMessenger.Default.Register<OpenLoadPatternMessage>(this, (_, _) =>
		{
			ShowLoadPatternCommand.Execute(null);
		});
		WeakReferenceMessenger.Default.Register<OpenCreatePatternMessage>(this, (_, _) =>
		{
			ShowCreatePatternCommand.Execute(null);
		});
		WeakReferenceMessenger.Default.Register<CloseLoadPatternMessage>(this, (_, _) =>
		{
			HideLoadPatternCommand.Execute(null);
		});


		BindingContext = new { viewModel, page = this };
	}

	private void CollapseTimerUI()
	{
		NextClockOverlay.ViewModel.CancelTimer();
		NextClockVisible = false;
	}

	#region Main Pattern Button UI
	public bool IsPatternSheetVisible
	{
		get => _isPatternSheetVisible;
		set
		{
			if (_isPatternSheetVisible != value)
			{
				_isPatternSheetVisible = value;
				OnPropertyChanged(nameof(IsPatternSheetVisible));
			}
		}
	}
	private bool _isPatternSheetVisible;

	public ICommand ShowPatternsMainViewCommand => new Command(async () =>
	{
		if (PatternMainHost.Content is null)
			PatternMainHost.Content = _patternMainpageView;

		// Reset vertical position before animating in
		CreatePatternContainer.TranslationY = 400;

		// Collapse flight bar
		_viewModel.SetFlightBarExpanded(false);

		IsPatternSheetVisible = true;
		await PatternSheetContainer.TranslateTo(0, 0, 300, Easing.SinOut);
	});

	public ICommand HidePatternSheetCommand => new Command(async () =>
	{
		await PatternSheetContainer.TranslateTo(0, 400, 250, Easing.SinIn);
		IsPatternSheetVisible = false;
	});
	#endregion

	#region Create Pattern Button UI
	private bool _isCreatePatternVisible;
	public bool IsCreatePatternVisible
	{
		get => _isCreatePatternVisible;
		set
		{
			if (_isCreatePatternVisible != value)
			{
				_isCreatePatternVisible = value;
				OnPropertyChanged(nameof(IsCreatePatternVisible));
			}
		}
	}
	public ICommand ShowCreatePatternCommand => new Command(async () =>
	{
		if (CreatePatternHost.Content is null)
			CreatePatternHost.Content = _createPatternView;

		// Reset vertical position before animating in
		CreatePatternContainer.TranslationY = 400;

		IsCreatePatternVisible = true;
		await CreatePatternContainer.TranslateTo(0, 0, 300, Easing.SinOut);
	});

	public ICommand HideCreatePatternCommand => new Command(async () =>
	{
		await CreatePatternContainer.TranslateTo(0, 400, 250, Easing.SinIn);
		IsCreatePatternVisible = false;
	});

	#endregion

	#region Load Pattern Button UI
	private bool _isLoadPatternVisible;
	public bool IsLoadPatternVisible
	{
		get => _isLoadPatternVisible;
		set
		{
			if (_isLoadPatternVisible != value)
			{
				_isLoadPatternVisible = value;
				OnPropertyChanged(nameof(IsLoadPatternVisible));
			}
		}
	}
	public ICommand ShowLoadPatternCommand => new Command(async () =>
	{
		if (LoadPatternHost.Content is null)
			LoadPatternHost.Content = _loadPatternView;

		LoadPatternContainer.TranslationY = 400;
		IsLoadPatternVisible = true;
		await LoadPatternContainer.TranslateTo(0, 0, 300, Easing.SinOut);
	});

	public ICommand HideLoadPatternCommand => new Command(async () =>
	{
		await LoadPatternContainer.TranslateTo(0, 400, 250, Easing.SinIn);
		IsLoadPatternVisible = false;
	});

	#endregion

	private async Task QuitAppAsync()
	{
		await Task.Delay(100); // Simulate some delay if needed
#if WINDOWS
		if (OperatingSystem.IsWindows())
		{
			// Windows-specific quit logic
			Application.Current?.Quit();
		}
#elif ANDROID
		if (OperatingSystem.IsAndroid())
		{
			// Android-specific quit logic			
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}
#elif IOS
		if (OperatingSystem.IsIOS())
		{
			// iOS-specific quit logic
			await DisplayAlert("Unsupported", "Please press the Home button to exit the app.", "OK");
		}
#else
		{
			// For other platforms, use the platform-specific APIs
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}
#endif
	}
}
