using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfEfCoreTest.Converters
{
    public class CharToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mess = "я не строка";
            if (value is char c) return c.ToString();
            return mess; //string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}