using System;
using Avalonia.Data.Converters;
using System.Globalization;

namespace InkHouse.Views
{
    public class BooleanToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? "可借" : "不可借";
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
                return s == "可借";
            return false;
        }
    }
} 