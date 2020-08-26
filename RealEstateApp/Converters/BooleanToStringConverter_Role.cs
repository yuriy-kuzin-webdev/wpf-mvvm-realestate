using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RealEstateApp.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToStringConverter_Role : IValueConverter
    {
        public static BooleanToStringConverter_Role Instance = new BooleanToStringConverter_Role();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value
                ? "Админ"
                : "Риелтор";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
