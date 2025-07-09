using Bingo.UI.Shared.Views.Patterns;
using Bingo.ViewModel.Patterns;
using System.Globalization;

namespace Bingo.UI.Shared.Converters;

public class SelectedPatternToStrokeConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (parameter is LoadPatternView root && root.BindingContext is LoadPatternViewModel vm)
			return vm.SelectedPattern == value ? Colors.Orange : Colors.Gray;

		return Colors.Transparent;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
