 using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace InkHouse.Views
{
    public class TitleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string title && parameter is string expected)
                return title == expected;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
