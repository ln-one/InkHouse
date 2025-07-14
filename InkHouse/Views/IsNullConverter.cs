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
            return value == null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 