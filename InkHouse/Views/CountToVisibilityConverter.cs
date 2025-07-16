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
                // 当count为0时显示"暂无记录"，否则隐藏
                return count == 0;
            }
            return true; // 默认显示"暂无记录"
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
} 