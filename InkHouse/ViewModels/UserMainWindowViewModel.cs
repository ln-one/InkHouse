using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using InkHouse.Models;
using InkHouse.Services;
using InkHouse.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System.Reactive.Linq;
using ReactiveUI;
using Avalonia.Threading;
using System.Collections.Generic;

namespace InkHouse.ViewModels
{
    public partial class UserMainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private object _currentView = "Home";

        [ObservableProperty]
        private string _selectedMenu = "Home";
        
        [ObservableProperty]
        private User? _currentUser;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _message = string.Empty;

        [ObservableProperty]
        private bool _showMessage = false;

        [ObservableProperty]
        private bool _isErrorMessage = false;

        public ObservableCollection<Book> Books { get; } = new();
        public ObservableCollection<BorrowRecord> BorrowRecords { get; } = new();

        [ObservableProperty]
        private int _currentPage = 1;

        [ObservableProperty]
        private int _pageSize = 20;

        [ObservableProperty]
        private int _totalBooks = 0;

        [ObservableProperty]
        private int _totalBorrowRecords = 0;

        public ICommand LogoutCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand BorrowBookCommand { get; }
        public IAsyncRelayCommand ShowBrowseBooksCommand { get; }
        public IAsyncRelayCommand ShowMyBorrowsCommand { get; }
        public IAsyncRelayCommand LoadMoreBooksCommand { get; }
        public IAsyncRelayCommand LoadMoreBorrowRecordsCommand { get; }

        // TODO: 依赖注入这些服务
        private readonly BookService _bookService;
        private readonly BorrowRecordService _borrowRecordService;
        private readonly SeatService _seatService;
        [ObservableProperty]
        private SeatReservationViewModel? _seatReservationViewModel;

        [ObservableProperty]
        private ObservableCollection<SeatReservation> _seatReservationRecords = new();
        [ObservableProperty]
        private ObservableCollection<BorrowRecord> _userBorrowRecords = new();

        // 统计信息属性
        [ObservableProperty]
        private int _totalBorrowCount = 0;
        [ObservableProperty]
        private int _totalReservationCount = 0;
        [ObservableProperty]
        private int _currentBorrowCount = 0;
        [ObservableProperty]
        private int _currentReservationCount = 0;
        [ObservableProperty]
        private int _returnedBorrowCount = 0;
        [ObservableProperty]
        private int _completedReservationCount = 0;
        [ObservableProperty]
        private DateTime _firstBorrowDate = DateTime.MinValue;
        [ObservableProperty]
        private DateTime _firstReservationDate = DateTime.MinValue;
        [ObservableProperty]
        private DateTime _lastBorrowDate = DateTime.MinValue;
        [ObservableProperty]
        private DateTime _lastReservationDate = DateTime.MinValue;

        public IAsyncRelayCommand LoadProfileDataCommand { get; }
        public IAsyncRelayCommand RefreshStatisticsCommand { get; }

        public async Task LoadProfileDataAsync()
        {
            if (CurrentUser == null) return;
            
            try
            {
                Console.WriteLine($"开始加载用户 {CurrentUser.UserName} (ID: {CurrentUser.Id}) 的个人资料数据...");
                
                // 加载座位预约记录
                var seatRecords = await _seatService.GetUserReservationsAsync(CurrentUser.Id);
                Console.WriteLine($"从数据库加载到 {seatRecords.Count} 条座位预约记录");
                SeatReservationRecords.Clear();
                foreach (var record in seatRecords)
                {
                    Console.WriteLine($"座位预约记录: ID={record.Id}, 座位={record.Seat?.SeatNumber}, 状态={record.Status}, 预约时间={record.ReserveTime}");
                    SeatReservationRecords.Add(record);
                }
                
                // 加载借阅记录
                var borrowRecords = await _borrowRecordService.GetBorrowRecordsByUserIdAsync(CurrentUser.Id, 1, 100);
                Console.WriteLine($"从数据库加载到 {borrowRecords.Count} 条借阅记录");
                UserBorrowRecords.Clear();
                foreach (var record in borrowRecords)
                {
                    Console.WriteLine($"借阅记录: ID={record.Id}, 图书={record.Book?.Title}, 状态={record.Status}, 借阅时间={record.BorrowDate}");
                    UserBorrowRecords.Add(record);
                }

                // 计算统计信息
                CalculateStatistics(seatRecords, borrowRecords);

                Console.WriteLine($"最终数据：座位预约记录 {SeatReservationRecords.Count} 条，借阅记录 {UserBorrowRecords.Count} 条");
                Console.WriteLine($"统计信息：总借阅 {TotalBorrowCount} 次，总预约 {TotalReservationCount} 次");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载个人资料数据失败: {ex.Message}");
                Console.WriteLine($"异常堆栈: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 计算用户统计信息
        /// </summary>
        private void CalculateStatistics(List<SeatReservation> seatRecords, List<BorrowRecord> borrowRecords)
        {
            // 借阅统计
            TotalBorrowCount = borrowRecords.Count;
            CurrentBorrowCount = borrowRecords.Count(r => r.Status == "借出" || r.Status == "借阅中");
            ReturnedBorrowCount = borrowRecords.Count(r => r.Status == "归还" || r.Status == "已归还");
            
            if (borrowRecords.Any())
            {
                FirstBorrowDate = borrowRecords.Min(r => r.BorrowDate);
                LastBorrowDate = borrowRecords.Max(r => r.BorrowDate);
            }

            // 预约统计
            TotalReservationCount = seatRecords.Count;
            CurrentReservationCount = seatRecords.Count(r => r.Status == "已预约" || r.Status == "使用中");
            CompletedReservationCount = seatRecords.Count(r => r.Status == "已离馆" || r.Status == "已完成");
            
            if (seatRecords.Any())
            {
                FirstReservationDate = seatRecords.Min(r => r.ReserveTime);
                LastReservationDate = seatRecords.Max(r => r.ReserveTime);
            }
        }

        public UserMainWindowViewModel(User user, BookService bookService, BorrowRecordService borrowRecordService, SeatService seatService)
        {
            CurrentUser = user;
            _bookService = bookService;
            _borrowRecordService = borrowRecordService;
            _seatService = seatService;
            SeatReservationViewModel = new SeatReservationViewModel(_seatService, user);

            LogoutCommand = new RelayCommand(Logout);
            SearchCommand = new AsyncRelayCommand(SearchBooksAsync);
            ReturnBookCommand = new AsyncRelayCommand<BorrowRecord>(ReturnBookAsync);
            BorrowBookCommand = new AsyncRelayCommand<Book>(BorrowBookAsync);
            ShowBrowseBooksCommand = new AsyncRelayCommand(ShowBrowseBooks);
            ShowMyBorrowsCommand = new AsyncRelayCommand(ShowMyBorrows);
            LoadMoreBooksCommand = new AsyncRelayCommand(LoadMoreBooks);
            LoadMoreBorrowRecordsCommand = new AsyncRelayCommand(LoadMoreBorrowRecords);
            LoadProfileDataCommand = new AsyncRelayCommand(LoadProfileDataAsync);
            RefreshStatisticsCommand = new AsyncRelayCommand(RefreshStatistics);
            // 已自动删除递归创建自身的代码，防止栈溢出
            // 默认显示主页
            ShowHome();
            // 自动加载主页统计数据
            _ = LoadBooksAsync();
            _ = LoadBorrowRecordsAsync();
            // 预加载个人中心数据
            _ = LoadProfileDataAsync();
        }

        [RelayCommand]
        private void ShowHome()
        {
            SelectedMenu = "Home";
            CurrentView = "Home";
        }

        public async Task ShowBrowseBooks()
        {
            SelectedMenu = "BrowseBooks";
            CurrentView = "BrowseBooks";
            await LoadBooksAsync();
        }

        public async Task ShowMyBorrows()
        {
            SelectedMenu = "MyBorrows";
            CurrentView = "MyBorrows";
            await LoadBorrowRecordsAsync();
            // 同步更新个人中心的借阅记录
            await LoadProfileDataAsync();
        }

        [RelayCommand]
        public async Task ShowSeatReservation()
        {
            SelectedMenu = "SeatReservation";
            CurrentView = "SeatReservation";
            // 同步更新个人中心的座位预约记录
            await LoadProfileDataAsync();
        }

        [RelayCommand]
        public async Task ShowMyProfile()
        {
            SelectedMenu = "MyProfile";
            var view = new InkHouse.Views.UserProfileView { DataContext = this };
            CurrentView = view;
            // 确保在设置DataContext后加载数据
            await LoadProfileDataAsync();
        }

        private async Task LoadBooksAsync()
        {
            try
            {
                IsLoading = true;
                var books = await _bookService.GetAllBooksAsync(CurrentPage, PageSize);
                
                // Get total count for the first time only (optimization)
                if (CurrentPage == 1 && TotalBooks == 0)
                {
                    // In a real app we'd have a count method, but for now just use this
                    TotalBooks = books.Count; // This is just an approximation for now
                }
                
                Books.Clear();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载图书失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchBooksAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                CurrentPage = 1; // Reset to first page when searching
                await LoadBooksAsync();
                return;
            }

            try
            {
                IsLoading = true;
                var books = await _bookService.SearchBooksAsync(SearchText, CurrentPage, PageSize);
                
                Books.Clear();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索图书失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadBorrowRecordsAsync()
        {
            if (CurrentUser == null) return;

            try
            {
                IsLoading = true;
                var records = await _borrowRecordService.GetBorrowRecordsByUserIdAsync(CurrentUser.Id, CurrentPage, PageSize);
                
                // Get total count for the first time only (optimization)
                if (CurrentPage == 1 && TotalBorrowRecords == 0)
                {
                    TotalBorrowRecords = records.Count; // This is just an approximation for now
                }
                
                BorrowRecords.Clear();
                foreach (var record in records)
                {
                    BorrowRecords.Add(record);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载借阅记录失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ReturnBookAsync(BorrowRecord? record)
        {
            if (record == null) return;

            try
            {
                await _borrowRecordService.ReturnBookAsync(record.Id);
                
                // Update the record locally instead of reloading everything
                record.ReturnDate = DateTime.Now;
                record.Status = "已归还";
                record.IsReturned = true;
                
                // Update the book availability if it's in the current view
                var book = Books.FirstOrDefault(b => b.Id == record.BookId);
                if (book != null)
                {
                    book.Available++;
                    book.IsAvailable = true;
                }
                
                // 同步更新个人中心的借阅记录
                await LoadProfileDataAsync();
                
                ShowSuccessMessage("还书成功！");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"还书失败: {ex.Message}");
            }
        }

        private async Task BorrowBookAsync(Book? book)
        {
            if (book == null || CurrentUser == null) return;

            try
            {
                var borrowRecord = await _borrowRecordService.BorrowBookAsync(book.Id, CurrentUser.Id);
                
                // Update book locally instead of reloading everything
                book.Available--;
                if (book.Available <= 0)
                {
                    book.IsAvailable = false;
                }
                
                // Add the borrow record to the collection if we're on the MyBorrows view
                if (CurrentView == "MyBorrows")
                {
                    BorrowRecords.Insert(0, borrowRecord);
                }
                
                // 同步更新个人中心的借阅记录
                await LoadProfileDataAsync();
                
                ShowSuccessMessage("借阅成功！");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"借阅失败: {ex.Message}");
            }
        }

        private void Logout()
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                // 隐藏当前主窗口
                desktop.MainWindow?.Hide();

                // 创建并显示新的登录窗口
                var loginWindow = new Views.LoginWindow();
                desktop.MainWindow = loginWindow;
                loginWindow.Show();
            }
        }

        private async void ShowSuccessMessage(string message)
        {
            Message = message;
            IsErrorMessage = false;
            ShowMessage = true;
            
            // 3秒后自动隐藏消息
            await Task.Delay(3000);
            ShowMessage = false;
        }

        private async void ShowErrorMessage(string message)
        {
            Message = message;
            IsErrorMessage = true;
            ShowMessage = true;
            
            // 5秒后自动隐藏错误消息
            await Task.Delay(5000);
            ShowMessage = false;
        }

        private async Task LoadMoreBooks()
        {
            CurrentPage++;
            await LoadBooksAsync();
        }

        private async Task LoadMoreBorrowRecords()
        {
            CurrentPage++;
            await LoadBorrowRecordsAsync();
        }

        private async Task RefreshStatistics()
        {
            await LoadProfileDataAsync();
        }
    }
}