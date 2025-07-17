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
                // Èç¹ûÈÕÆÚÊÇÄ¬ÈÏÖµ£¨DateTime.MinValue£©£¬ÏÔÊ¾"ÔÝÎÞ"
                if (dateTime == DateTime.MinValue)
                {
                    return "ÔÝÎÞ";
                }
                
                // ·ñÔò°´Ö¸¶¨¸ñÊ½ÏÔÊ¾ÈÕÆÚ
                string format = parameter as string ?? "yyyy-MM-dd";
                return dateTime.ToString(format);
            }
            
            return "ÔÝÎÞ";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 