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

        public ICommand LogoutCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand BorrowBookCommand { get; }

        // TODO: 依赖注入这些服务
        private readonly BookService _bookService;
        private readonly BorrowRecordService _borrowRecordService;

        public UserMainWindowViewModel(User user, BookService bookService, BorrowRecordService borrowRecordService)
        {
            CurrentUser = user;
            _bookService = bookService;
            _borrowRecordService = borrowRecordService;

            LogoutCommand = new RelayCommand(Logout);
            SearchCommand = new AsyncRelayCommand(SearchBooksAsync);
            ReturnBookCommand = new AsyncRelayCommand<BorrowRecord>(ReturnBookAsync);
            BorrowBookCommand = new AsyncRelayCommand<Book>(BorrowBookAsync);

            // 默认显示主页
            ShowHome();
        }

        [RelayCommand]
        private void ShowHome()
        {
            SelectedMenu = "Home";
            CurrentView = "Home";
        }

        [RelayCommand]
        private async Task ShowBrowseBooks()
        {
            SelectedMenu = "BrowseBooks";
            CurrentView = "BrowseBooks";
            await LoadBooksAsync();
        }

        [RelayCommand]
        private async Task ShowMyBorrows()
        {
            SelectedMenu = "MyBorrows";
            CurrentView = "MyBorrows";
            await LoadBorrowRecordsAsync();
        }

        [RelayCommand]
        private void ShowSeatReservation()
        {
            SelectedMenu = "SeatReservation";
            CurrentView = "SeatReservation";
        }

        [RelayCommand]
        private void ShowMyProfile()
        {
            SelectedMenu = "MyProfile";
            CurrentView = "MyProfile";
        }

        private async Task LoadBooksAsync()
        {
            try
            {
                IsLoading = true;
                var books = await _bookService.GetAllBooksAsync();
                
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
                await LoadBooksAsync();
                return;
            }

            try
            {
                IsLoading = true;
                var books = await _bookService.SearchBooksAsync(SearchText);
                
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
                var records = await _borrowRecordService.GetBorrowRecordsByUserIdAsync(CurrentUser.Id);
                
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
                await LoadBorrowRecordsAsync(); // 重新加载借阅记录
                await LoadBooksAsync(); // 重新加载图书列表（更新可用状态）
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
                await _borrowRecordService.BorrowBookAsync(book.Id, CurrentUser.Id);
                await LoadBooksAsync(); // 重新加载图书列表（更新可用状态）
                await LoadBorrowRecordsAsync(); // 重新加载借阅记录
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
    }
}