using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RealEstateApp.Converters
{
    public class ReservedConfirmedToBooleanConverter : IValueConverter
    {
        private const string ConfirmedText = "Выкуплена";
        private const string ReservedText = "Забронирована";
        public static ReservedConfirmedToBooleanConverter Instance = new ReservedConfirmedToBooleanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(true, value)
                ? ConfirmedText
                : ReservedText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, ConfirmedText);
        }
    }
}
