using System.Globalization;

namespace Bingo.UI.Shared.Converters
{
    public class BoolToCellBackgroundConverter : IValueConverter
    {
        public Brush CalledBrush { get; set; } = Colors.Black;
        public Brush UncalledBrush { get; set; } = Colors.White;

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            value is true ? CalledBrush : UncalledBrush;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }

    public class BoolToCellTextColorConverter : IValueConverter
    {
        public Color CalledColor { get; set; } = Colors.White;
        public Color UncalledColor { get; set; } = Colors.Black;

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            value is true ? CalledColor : UncalledColor;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
