using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace InkHouse.Views
{
    public class MessageBackgroundConverter : IValueConverter
    {
        public static readonly MessageBackgroundConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isErrorMessage)
            {
                return isErrorMessage ? new SolidColorBrush(Color.Parse("#F44336")) : new SolidColorBrush(Color.Parse("#4CAF50"));
            }
            
            return new SolidColorBrush(Color.Parse("#4CAF50")); // 默认绿色
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 