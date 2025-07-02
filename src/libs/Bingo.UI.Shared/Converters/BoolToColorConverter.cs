using System.Globalization;

namespace Bingo.UI.Shared.Converters;

public class BoolToColorConverter : IValueConverter
{
	public Color ActiveColor { get; set; } = Colors.OrangeRed;
	public Color InactiveColor { get; set; } = Colors.LightGray;

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value is bool b && b ? ActiveColor : InactiveColor;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
