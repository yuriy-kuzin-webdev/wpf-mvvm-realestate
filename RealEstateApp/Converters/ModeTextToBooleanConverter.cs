using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RealEstateApp.Converters
{
    public class ModeTextToBooleanConverter : IValueConverter
    {
        private const string ModeOnText = "Обновление";
        private const string ModeOfText = "Сохранение";
        public static ModeTextToBooleanConverter Instance = new ModeTextToBooleanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(true, value)
                ? ModeOnText
                : ModeOfText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, ModeOnText);
        }
    }
}
