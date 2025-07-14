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

        public ObservableCollection<Book> Books { get; } = new();

        public ICommand LogoutCommand { get; }
        public ICommand SearchCommand { get; }

        // TODO: 依赖注入这些服务
        private readonly BookService _bookService;

        public UserMainWindowViewModel(User user, BookService bookService)
        {
            _currentUser = user;
            _bookService = bookService;

            LogoutCommand = new RelayCommand(Logout);
            SearchCommand = new AsyncRelayCommand(SearchBooksAsync);

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
        private void ShowMyBorrows()
        {
            SelectedMenu = "MyBorrows";
            CurrentView = "MyBorrows";
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
    }
}