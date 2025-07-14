using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using InkHouse.Models;
// using InkHouse.Services.Interfaces; // 等待实现时再取消注释
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
    // 用于设计时预览的图书模型
    public class DesignBook
    {
        public string Title { get; set; } = "Book Title";
        public string Author { get; set; } = "Author Name";
        public string CoverUrl { get; set; } = "https://img3.doubanio.com/view/subject/s/public/s33943361.jpg"; // 示例封面
        public bool IsAvailable { get; set; } = true;
    }

    public partial class UserMainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _currentView = "Home";

        [ObservableProperty]
        private string _selectedMenu = "Home";
        
        [ObservableProperty]
        private User? _currentUser;

        public ObservableCollection<DesignBook> Books { get; }

        public ICommand LogoutCommand { get; }

        // TODO: 依赖注入这些服务
        // private readonly IUserBookService _bookService;
        // private readonly IUserBorrowService _borrowService;
        // private readonly IUserAccountService _accountService;

        public UserMainWindowViewModel(User user)
        {
            _currentUser = user;
            // _bookService = bookService;
            // _borrowService = borrowService;
            // _accountService = accountService;

            LogoutCommand = new RelayCommand(Logout);

            // 为设计时预览创建示例数据
            Books = new ObservableCollection<DesignBook>
            {
                new() { Title = "三体", Author = "刘慈欣", IsAvailable = true },
                new() { Title = "流浪地球", Author = "刘慈欣", IsAvailable = false },
                new() { Title = "活着", Author = "余华", IsAvailable = true },
                new() { Title = "许三观卖血记", Author = "余华", IsAvailable = true },
                new() { Title = "代码整洁之道", Author = "Robert C. Martin", IsAvailable = false },
                new() { Title = "深入理解计算机系统", Author = "Randal E. Bryant", IsAvailable = true },
            };

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
        private void ShowBrowseBooks()
        {
            SelectedMenu = "BrowseBooks";
            CurrentView = "BrowseBooks";
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