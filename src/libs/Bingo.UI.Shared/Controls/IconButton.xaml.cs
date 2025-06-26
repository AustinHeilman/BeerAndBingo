using Bingo.UI.Shared.Helpers;
using System.Windows.Input;

namespace Bingo.UI.Shared.Controls;

public partial class IconButton : ContentView
{
	public IconButton()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty IconNameProperty =
		BindableProperty.Create(nameof(IconName), typeof(string), typeof(IconButton), propertyChanged: OnIconNameChanged);

	public string IconName
	{
		get => (string)GetValue(IconNameProperty);
		set => SetValue(IconNameProperty, value);
	}

	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(IconButton));

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public static readonly BindableProperty IconSizeProperty =
		BindableProperty.Create(nameof(IconSize), typeof(double), typeof(IconButton), 48.0);

	public double IconSize
	{
		get => (double)GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

	public event EventHandler? LongPressed;

	private CancellationTokenSource? _longPressCts;

	private static void OnIconNameChanged(BindableObject bindable, object _, object newValue)
	{
		if (bindable is IconButton control && newValue is string iconName)
			control.Icon.Source = PlatformIconHelper.Get(iconName);
	}

	private async void OnTapped(object? sender, EventArgs e)
	{
		// Animate press
		await this.ScaleTo(0.92, 50, Easing.CubicOut);

		// Begin long press detection
		_longPressCts = new CancellationTokenSource();
		CancellationToken token = _longPressCts.Token;

		bool longPressFired = false;

		_ = Task.Run(async () =>
		{
			try
			{
				await Task.Delay(500, token);
				longPressFired = true;
				MainThread.BeginInvokeOnMainThread(() =>
					LongPressed?.Invoke(this, EventArgs.Empty));
			}
			catch (TaskCanceledException) { /* tap happened */ }
		});

		// Wait a short moment before scaling back
		await this.ScaleTo(1.0, 50, Easing.CubicIn);

		// If user released too early, treat as normal tap
		if (!longPressFired && Command?.CanExecute(null) == true)
			Command.Execute(null);

		_longPressCts.Cancel();
	}
}
