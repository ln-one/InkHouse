using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace InkHouse.Views
{
    public class SeatStatusToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var status = value?.ToString();
            return status switch
            {
                "Free" => new SolidColorBrush(Color.FromRgb(0x6F, 0xD6, 0x57)), // ÂÌÉ«
                "Reserved" => new SolidColorBrush(Color.FromRgb(0xFF, 0xA5, 0x00)), // ³ÈÉ«
                "Occupied" => new SolidColorBrush(Color.FromRgb(0xF5, 0x4A, 0x4A)), // ºìÉ«
                _ => new SolidColorBrush(Colors.Gray)
            };
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
} 