using System;
using System.Globalization;
using Avalonia.Data.Converters;
using InkHouse.Models;

namespace InkHouse.Views
{
    public class BorrowStatusConverter : IValueConverter
    {
        public static readonly BorrowStatusConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is BorrowRecord record)
            {
                if (record.ReturnDate.HasValue)
                {
                    return $"已归还 ({record.ReturnDate.Value:yyyy-MM-dd})";
                }
                else
                {
                    // 计算应还日期：借阅日期 + 30天
                    var dueDate = record.BorrowDate.AddDays(30);
                    return $"借阅中 (应还: {dueDate:yyyy-MM-dd})";
                }
            }
            
            return "未知状态";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 