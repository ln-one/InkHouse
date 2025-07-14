using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace InkHouse.Views
{
    public class DueDateConverter : IValueConverter
    {
        public static readonly DueDateConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime borrowDate)
            {
                var dueDate = borrowDate.AddDays(30);
                return dueDate.ToString("yyyy-MM-dd");
            }
            
            return "未知";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 