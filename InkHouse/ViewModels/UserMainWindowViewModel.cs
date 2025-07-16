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
        private string _currentView = "Home";

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

        [ObservableProperty]
        private List<string> _bookTypes = new();
        [ObservableProperty]
        private string _selectedBookType = "全部";
        partial void OnSelectedBookTypeChanged(string? value)
        {
            CurrentPage = 1;
            _ = LoadBooksAsync();
        }

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
            // 已自动删除递归创建自身的代码，防止栈溢出
            // 默认显示主页
            ShowHome();
            // 自动加载主页统计数据
            _ = LoadBooksAsync();
            _ = LoadBorrowRecordsAsync();
            _ = LoadBookTypesAsync();
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
        }

        [RelayCommand]
        public void ShowSeatReservation()
        {
            SelectedMenu = "SeatReservation";
            CurrentView = "SeatReservation";
        }

        [RelayCommand]
        public void ShowMyProfile()
        {
            SelectedMenu = "MyProfile";
            CurrentView = "MyProfile";
        }

        private async Task LoadBooksAsync()
        {
            try
            {
                IsLoading = true;
                var books = await _bookService.GetBooksByTypeAsync(SelectedBookType, CurrentPage, PageSize);
                
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
                    book.AvailableCount++;
                    book.IsAvailable = true;
                }
                
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
                book.AvailableCount--;
                if (book.AvailableCount <= 0)
                {
                    book.IsAvailable = false;
                }
                
                // Add the borrow record to the collection if we're on the MyBorrows view
                if (CurrentView == "MyBorrows")
                {
                    BorrowRecords.Insert(0, borrowRecord);
                }
                
                ShowSuccessMessage("借阅成功！");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"借阅失败: {ex.Message}");
            }
        }

        private async Task LoadBookTypesAsync()
        {
            var types = await _bookService.GetAllBookTypesAsync();
            types.Insert(0, "全部");
            BookTypes = types;
        }

        private async Task ChangeBookTypeAsync(string? type)
        {
            SelectedBookType = type ?? "全部";
            CurrentPage = 1;
            await LoadBooksAsync();
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
    }
}