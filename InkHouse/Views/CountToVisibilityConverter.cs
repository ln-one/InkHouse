using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace InkHouse.Views
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                // µ±countÎª0Ê±ÏÔÊ¾"ÔÝÎÞ¼ÇÂ¼"£¬·ñÔòÒþ²Ø
                return count == 0;
            }
            return true; // Ä¬ÈÏÏÔÊ¾"ÔÝÎÞ¼ÇÂ¼"
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
} 