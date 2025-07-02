using System.Globalization;

namespace Bingo.UI.Shared.Converters;

public class InverseBoolConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value is bool b ? !b : value;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value is bool b ? !b : value;
	}
}
