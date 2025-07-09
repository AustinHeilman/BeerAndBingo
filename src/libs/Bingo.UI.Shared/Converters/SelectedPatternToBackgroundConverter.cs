using Bingo.UI.Shared.Views.Patterns;
using Bingo.ViewModel.Patterns;
using System.Globalization;

namespace Bingo.UI.Shared.Converters;

public class SelectedPatternToBackgroundConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (parameter is LoadPatternView root && root.BindingContext is LoadPatternViewModel vm)
		{
			if (vm.SelectedPattern == value)
				return Color.FromArgb("#FFE4B5"); // Light orange highlight
			return Application.Current?.RequestedTheme == AppTheme.Dark
				? Color.FromArgb("#222")
				: Color.FromArgb("#F7F7F7");
		}
		return Colors.Transparent;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}

