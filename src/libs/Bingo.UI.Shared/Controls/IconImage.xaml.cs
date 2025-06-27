using Bingo.UI.Shared.Helpers;

namespace Bingo.UI.Shared.Controls;

public partial class IconImage : ContentView
{
	public IconImage()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty IconNameProperty =
		BindableProperty.Create(nameof(IconName), typeof(string), typeof(IconImage), propertyChanged: OnIconNameChanged);

	public string IconName
	{
		get => (string)GetValue(IconNameProperty);
		set => SetValue(IconNameProperty, value);
	}

	private static void OnIconNameChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is IconImage control && newValue is string iconName)
		{
			control.Icon.Source = PlatformIconHelper.Get(iconName);
		}
	}
}
