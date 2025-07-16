using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace InkHouse.Views
{
    public class DateDisplayConverter : IValueConverter
    {
        public static readonly DateDisplayConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                // 如果日期是默认值（DateTime.MinValue），显示"暂无"
                if (dateTime == DateTime.MinValue)
                {
                    return "暂无";
                }
                
                // 否则按指定格式显示日期
                string format = parameter as string ?? "yyyy-MM-dd";
                return dateTime.ToString(format);
            }
            
            return "暂无";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 