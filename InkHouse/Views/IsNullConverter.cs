using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace InkHouse.Views
{
    public class IsNullConverter : IValueConverter
    {
        public static readonly IsNullConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // 修复：返回值不为null，而不是为null
            return value != null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 