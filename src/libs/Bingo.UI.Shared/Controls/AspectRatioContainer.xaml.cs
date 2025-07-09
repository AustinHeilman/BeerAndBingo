using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Bingo.UI.Shared.Controls;

public partial class AspectRatioContainer : ContentView
{
	public AspectRatioContainer()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty RatioProperty =
		BindableProperty.Create(nameof(Ratio), typeof(double), typeof(AspectRatioContainer), 1.0);

	public double Ratio
	{
		get => (double)GetValue(RatioProperty);
		set => SetValue(RatioProperty, value);
	}

	protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
	{
		double min = Math.Min(widthConstraint, heightConstraint);
		double width = min;
		double height = min / Ratio;

		return new Size(width, height);
	}
}
