using System;
using Avalonia.Data.Converters;
using System.Globalization;

namespace InkHouse.Views
{
    public class MenuCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string selected && parameter is string menu)
                return selected == menu;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b && parameter is string menu)
                return menu;
            return Avalonia.Data.BindingOperations.DoNothing;
        }
    }
} 