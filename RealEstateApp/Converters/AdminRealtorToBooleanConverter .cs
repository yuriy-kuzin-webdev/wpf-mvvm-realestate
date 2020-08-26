using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RealEstateApp.Converters
{
    public class AdminRealtorToBooleanConverter : IValueConverter
    {
        private const string AdminText = "Админ";
        private const string RealtorText = "Риелтор";
        public static AdminRealtorToBooleanConverter Instance = new AdminRealtorToBooleanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(true, value)
                ? AdminText
                : RealtorText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, AdminText);
        }
    }
}
