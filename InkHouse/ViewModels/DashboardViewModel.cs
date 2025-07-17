using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InkHouse.Models;
using InkHouse.Services;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace InkHouse.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase
    {
        private readonly BookService _bookService;
        private readonly UserService _userService;
        private readonly BorrowRecordService _borrowRecordService;
        private readonly SeatService _seatService;

        // 统计数据
        [ObservableProperty] private int _totalBooks;
        [ObservableProperty] private int _availableBooks;
        [ObservableProperty] private int _borrowedBooks;
        [ObservableProperty] private int _registeredUsers;
        [ObservableProperty] private int _totalBorrows;
        [ObservableProperty] private int _activeBorrows;
        [ObservableProperty] private int _returnedBorrows;
        [ObservableProperty] private int _totalSeats;
        [ObservableProperty] private int _freeSeats;
        [ObservableProperty] private int _reservedSeats;
        [ObservableProperty] private int _occupiedSeats;

        // 图表数据
        [ObservableProperty] private ISeries[] _bookStatusSeries = Array.Empty<ISeries>();
        [ObservableProperty] private ISeries[] _borrowTrendSeries = Array.Empty<ISeries>();
        [ObservableProperty] private ISeries[] _seatStatusSeries = Array.Empty<ISeries>();
        
        // 图表轴标签
        [ObservableProperty] private List<string> _monthLabels = new();

        // 最近借阅记录
        [ObservableProperty] private ObservableCollection<BorrowRecord> _recentBorrowRecords = new();

        // 热门图书
        [ObservableProperty] private ObservableCollection<BookPopularity> _popularBooks = new();

        public DashboardViewModel(BookService bookService, UserService userService, 
            BorrowRecordService borrowRecordService, SeatService seatService)
        {
            _bookService = bookService;
            _userService = userService;
            _borrowRecordService = borrowRecordService;
            _seatService = seatService;

            // 初始化月份标签
            MonthLabels = new List<string> { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };

            // 延迟加载数据
            Dispatcher.UIThread.Post(async () =>
            {
                await LoadDashboardDataAsync();
            });
        }

        [RelayCommand]
        public async Task LoadDashboardDataAsync()
        {
            await ExecuteAsync(async () =>
            {
                // 加载基本统计数据
                await LoadStatisticsAsync();
                
                // 加载图表数据
                await LoadBookStatusChartAsync();
                await LoadBorrowTrendChartAsync();
                await LoadSeatStatusChartAsync();
                
                // 加载最近借阅记录
                await LoadRecentBorrowRecordsAsync();
                
                // 加载热门图书
                await LoadPopularBooksAsync();
            });
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                // 加载图书统计
                var (totalBooks, availableBooks, borrowedBooks) = await _bookService.GetBookStatisticsAsync();
                TotalBooks = totalBooks;
                AvailableBooks = availableBooks;
                BorrowedBooks = borrowedBooks;
                
                Console.WriteLine($"图书统计 - 总数: {TotalBooks}, 可借: {AvailableBooks}, 已借: {BorrowedBooks}");
                
                // 加载用户统计
                var users = await _userService.GetAllUsersAsync();
                RegisteredUsers = users.Count;
                
                Console.WriteLine($"用户统计 - 总数: {RegisteredUsers}");
                
                // 加载借阅统计
                var (totalBorrows, activeBorrows, returnedBorrows) = await _borrowRecordService.GetBorrowStatisticsAsync();
                TotalBorrows = totalBorrows;
                ActiveBorrows = activeBorrows;
                ReturnedBorrows = returnedBorrows;
                
                Console.WriteLine($"借阅统计 - 总数: {TotalBorrows}, 未归还: {ActiveBorrows}, 已归还: {ReturnedBorrows}");
                
                // 加载座位统计
                var (totalSeats, freeSeats, reservedSeats, occupiedSeats) = await _seatService.GetSeatStatisticsAsync();
                TotalSeats = totalSeats;
                FreeSeats = freeSeats;
                ReservedSeats = reservedSeats;
                OccupiedSeats = occupiedSeats;
                
                Console.WriteLine($"座位统计 - 总数: {TotalSeats}, 空闲: {FreeSeats}, 已预约: {ReservedSeats}, 使用中: {OccupiedSeats}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载统计数据失败: {ex.Message}");
                
                // 设置默认值以防止显示问题
                TotalBooks = 0;
                AvailableBooks = 0;
                BorrowedBooks = 0;
                RegisteredUsers = 0;
                TotalBorrows = 0;
                ActiveBorrows = 0;
                ReturnedBorrows = 0;
                TotalSeats = 0;
                FreeSeats = 0;
                ReservedSeats = 0;
                OccupiedSeats = 0;
            }
        }

        private Task LoadBookStatusChartAsync()
        {
            // 图书状态饼图
            BookStatusSeries = new ISeries[]
            {
                new PieSeries<double>
                {
                    Values = new double[] { AvailableBooks },
                    Name = "Available",
                    Fill = new SolidColorPaint(SKColors.MediumSeaGreen),
                    Stroke = null,
                    DataLabelsSize = 14
                },
                new PieSeries<double>
                {
                    Values = new double[] { BorrowedBooks },
                    Name = "Borrowed",
                    Fill = new SolidColorPaint(SKColors.Crimson),
                    Stroke = null,
                    DataLabelsSize = 14
                }
            };
            
            return Task.CompletedTask;
        }

        private Task LoadBorrowTrendChartAsync()
        {
            // 模拟过去12个月的借阅趋势数据
            // 实际应用中应从数据库查询
            var random = new Random();
            var borrowData = new List<double>();
            var returnData = new List<double>();
            
            // 生成一些看起来合理的数据，确保有一些趋势
            int baseValue = random.Next(15, 30);
            for (int i = 0; i < 12; i++)
            {
                // 添加一些季节性变化
                double seasonalFactor = 1.0;
                if (i >= 8 || i <= 1) // 9-2月（学期开始和结束）
                    seasonalFactor = 1.5;
                else if (i >= 5 && i <= 7) // 6-8月（暑假）
                    seasonalFactor = 0.7;
                
                int borrow = (int)(baseValue * seasonalFactor + random.Next(-5, 10));
                borrowData.Add(Math.Max(5, borrow)); // 确保至少有5本
                
                // 归还数量通常略少于借出数量
                int returnCount = (int)(borrow * 0.8 + random.Next(-3, 4));
                returnData.Add(Math.Max(3, returnCount)); // 确保至少有3本
                
                // 调整基准值，添加一些趋势
                baseValue += random.Next(-2, 3);
                baseValue = Math.Max(10, Math.Min(40, baseValue)); // 保持在合理范围内
            }
            
            BorrowTrendSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = borrowData.ToArray(),
                    Name = "Borrow",
                    Fill = null,
                    GeometryFill = new SolidColorPaint(SKColors.DodgerBlue),
                    Stroke = new SolidColorPaint(SKColors.DodgerBlue, 3),
                    GeometrySize = 8,
                    LineSmoothness = 0.5 // 添加平滑曲线效果
                },
                new LineSeries<double>
                {
                    Values = returnData.ToArray(),
                    Name = "Return",
                    Fill = null,
                    GeometryFill = new SolidColorPaint(SKColors.Orange),
                    Stroke = new SolidColorPaint(SKColors.Orange, 3),
                    GeometrySize = 8,
                    LineSmoothness = 0.5 // 添加平滑曲线效果
                }
            };
            
            return Task.CompletedTask;
        }

        private Task LoadSeatStatusChartAsync()
        {
            // 座位状态饼图
            SeatStatusSeries = new ISeries[]
            {
                new PieSeries<double>
                {
                    Values = new double[] { FreeSeats },
                    Name = "Free",
                    Fill = new SolidColorPaint(SKColors.MediumSeaGreen),
                    Stroke = null,
                    DataLabelsSize = 14
                },
                new PieSeries<double>
                {
                    Values = new double[] { ReservedSeats },
                    Name = "Reserved",
                    Fill = new SolidColorPaint(SKColors.DodgerBlue),
                    Stroke = null,
                    DataLabelsSize = 14
                },
                new PieSeries<double>
                {
                    Values = new double[] { OccupiedSeats },
                    Name = "Occupied",
                    Fill = new SolidColorPaint(SKColors.Orange),
                    Stroke = null,
                    DataLabelsSize = 14
                }
            };
            
            return Task.CompletedTask;
        }



        private async Task LoadRecentBorrowRecordsAsync()
        {
            var records = await _borrowRecordService.GetAllBorrowRecordsAsync();
            var recentRecords = records
                .OrderByDescending(r => r.BorrowDate)
                .Take(5)
                .ToList();
            
            RecentBorrowRecords.Clear();
            foreach (var record in recentRecords)
            {
                RecentBorrowRecords.Add(record);
            }
        }

        private async Task LoadPopularBooksAsync()
        {
            // 模拟热门图书数据
            // 实际应用中应从数据库查询借阅次数最多的图书
            var books = await _bookService.GetAllBooksAsync();
            var random = new Random();
            
            var popularBooks = books
                .Take(5)
                .Select(b => new BookPopularity
                {
                    Book = b,
                    BorrowCount = random.Next(5, 50)
                })
                .OrderByDescending(bp => bp.BorrowCount)
                .ToList();
            
            PopularBooks.Clear();
            foreach (var book in popularBooks)
            {
                PopularBooks.Add(book);
            }
        }
    }

    // 图书热门度模型
    public class BookPopularity
    {
        public required Book Book { get; set; }
        public int BorrowCount { get; set; }
    }
}