using System.Globalization;

namespace Bingo.UI.Shared.Converters
{
	public class IconSourceConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is string iconName)
			{
				// Return image from file — adjust logic as needed
				return ImageSource.FromFile($"{iconName}.png");
			}

			return null; // CS8603 resolved: explicitly nullable
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			// You don't need reverse conversion here
			throw new NotImplementedException();
		}
	}
}
