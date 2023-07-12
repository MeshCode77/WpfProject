using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfEfCoreTest.Converters
{
    internal class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            if (boolValue)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibilityValue = (Visibility)value;
            return visibilityValue == Visibility.Visible;
        }
    }
}