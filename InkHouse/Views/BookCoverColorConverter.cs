using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace InkHouse.Views
{
    public class BookCoverColorConverter : IValueConverter
    {
        public static readonly BookCoverColorConverter Instance = new();

        // 预定义的颜色数组，用于生成不同的封面颜色
        private static readonly SolidColorBrush[] CoverColors = {
            new SolidColorBrush(Color.FromRgb(52, 152, 219)),   // 蓝色
            new SolidColorBrush(Color.FromRgb(231, 76, 60)),    // 红色
            new SolidColorBrush(Color.FromRgb(46, 204, 113)),   // 绿色
            new SolidColorBrush(Color.FromRgb(155, 89, 182)),   // 紫色
            new SolidColorBrush(Color.FromRgb(241, 196, 15)),   // 黄色
            new SolidColorBrush(Color.FromRgb(230, 126, 34)),   // 橙色
            new SolidColorBrush(Color.FromRgb(26, 188, 156)),   // 青色
            new SolidColorBrush(Color.FromRgb(142, 68, 173)),   // 深紫色
            new SolidColorBrush(Color.FromRgb(39, 174, 96)),    // 深绿色
            new SolidColorBrush(Color.FromRgb(192, 57, 43)),    // 深红色
        };

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string title)
            {
                // 根据书名生成哈希值，然后选择对应的颜色
                int hash = title.GetHashCode();
                int colorIndex = Math.Abs(hash) % CoverColors.Length;
                return CoverColors[colorIndex];
            }
            
            // 默认颜色
            return new SolidColorBrush(Color.FromRgb(149, 165, 166));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 