using System.Windows.Input;

namespace Bingo.UI.Shared.Controls;

public partial class IconButton : ContentView
{
	public IconButton()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty IconNameProperty =
		BindableProperty.Create(nameof(IconName), typeof(string), typeof(IconButton), default(string));

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
		BindableProperty.Create(nameof(IconSize), typeof(double), typeof(IconButton), 24.0);

	public double IconSize
	{
		get => (double)GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

	private async void OnPressed(object? sender, EventArgs e)
	{
		await this.ScaleTo(0.92, 50, Easing.CubicOut);
	}

	private async void OnReleased(object? sender, EventArgs e)
	{
		await this.ScaleTo(1.0, 50, Easing.CubicIn);
	}
}
