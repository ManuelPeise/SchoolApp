using Shared.Enums;
using System.Globalization;

namespace Web.App.Converters
{
    public class UserRoleToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return false;
            }

            var userRole = Enum.Parse(typeof(UserRoleEnum), value as string);

            var requiredRole = Enum.Parse(typeof(UserRoleEnum), parameter as string);

            return (int)userRole == (int)requiredRole;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
