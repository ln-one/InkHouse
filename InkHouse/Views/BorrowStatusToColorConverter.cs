
using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using InkHouse.Models;

namespace InkHouse.Views
{
    public class BorrowStatusToColorConverter : IValueConverter
    {
        public static readonly BorrowStatusToColorConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is BorrowRecord record)
            {
                if (record.ReturnDate.HasValue)
                {
                    return Brushes.Green; // Returned
                }
                
                var dueDate = record.BorrowDate.AddDays(30);
                if (DateTime.Now > dueDate)
                {
                    return Brushes.Red; // Overdue
                }
                
                return Brushes.Orange; // Borrowing
            }
            
            return Brushes.Black; // Default/Unknown
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
