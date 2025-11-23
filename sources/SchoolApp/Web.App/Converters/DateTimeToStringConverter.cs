using System.Globalization;

namespace Web.App.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var dateValue = (DateTime?)value;

            if (dateValue.HasValue)
            {
                return dateValue.Value.ToString("dd.MM.yyyy");
            }

            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var dateValue = value as string;
            
            if (DateTime.TryParseExact(dateValue, "dd.MM.yyyy", culture, DateTimeStyles.None, out var result))
            {
                return result;
            }
            
            return null;
        }
    }
}
