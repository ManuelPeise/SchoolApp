using System.Globalization;

namespace Web.App.Converters
{
    public class BooleanToKeyboardConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isNumber && isNumber)
                return Keyboard.Numeric;

            return Keyboard.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
