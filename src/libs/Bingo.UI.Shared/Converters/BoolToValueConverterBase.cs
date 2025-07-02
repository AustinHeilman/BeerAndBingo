using System.Globalization;

namespace Bingo.UI.Shared.Converters;

public abstract class BoolToValueConverterBase<T> : IValueConverter
{
	public T? TrueValue { get; set; }
	public T? FalseValue { get; set; }

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		T? result = value is true ? TrueValue : FalseValue;

		if (result == null)
		{
			//Debug.WriteLine($"[BoolToValueConverterBase] Null value for targetType {targetType}, returning UnsetValue");
			return BindableProperty.UnsetValue;
		}

		if (targetType == typeof(Color) && result is SolidColorBrush sb)
		{
			//Debug.WriteLine($"[BoolToValueConverterBase] Unwrapping SolidColorBrush → {sb.Color}");
			return sb.Color;
		}

		return result;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}
