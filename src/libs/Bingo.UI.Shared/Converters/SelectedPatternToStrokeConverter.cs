using Bingo.UI.Shared.Views.Patterns;
using Bingo.ViewModel.Patterns;
using System.Globalization;

namespace Bingo.UI.Shared.Converters;

public class SelectedPatternToStrokeConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var selected = (parameter as LoadPatternView)?.BindingContext as LoadPatternViewModel;
		return selected?.SelectedPattern == value ? Colors.Goldenrod : Colors.Gray;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
