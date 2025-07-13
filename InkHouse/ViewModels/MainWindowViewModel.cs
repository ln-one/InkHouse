using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using InkHouse.Models;
using InkHouse.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;

namespace InkHouse.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // 欢迎语
    public string Greeting { get; } = "Welcome to Avalonia!";

    // ================== 仪表板统计 ==================
    /// <summary>总藏书量</summary>
    public int TotalBooks { get; set; }
    /// <summary>可借图书</summary>
    public int AvailableBooks { get; set; }
    /// <summary>借出图书</summary>
    public int BorrowedBooks { get; set; }
    /// <summary>注册用户</summary>
    public int RegisteredUsers { get; set; }

    // ================== 图书管理 ==================
    /// <summary>图书列表</summary>
    public ObservableCollection<Book> Books { get; set; } = new();
    /// <summary>加载图书列表</summary>
    public Task LoadBooksAsync() => Task.CompletedTask;
    /// <summary>添加图书</summary>
    public Task AddBookAsync(Book book) => Task.CompletedTask;
    /// <summary>编辑图书</summary>
    public Task EditBookAsync(Book book) => Task.CompletedTask;
    /// <summary>删除图书</summary>
    public Task DeleteBookAsync(Book book) => Task.CompletedTask;

    // ================== 用户管理 ==================
    /// <summary>用户列表</summary>
    public ObservableCollection<User> Users { get; set; } = new();
    /// <summary>加载用户列表</summary>
    public Task LoadUsersAsync() => Task.CompletedTask;
    /// <summary>添加用户</summary>
    public Task AddUserAsync(User user) => Task.CompletedTask;
    /// <summary>编辑用户</summary>
    public Task EditUserAsync(User user) => Task.CompletedTask;
    /// <summary>删除用户</summary>
    public Task DeleteUserAsync(User user) => Task.CompletedTask;

    // ================== 借阅管理 ==================
    /// <summary>借阅记录列表</summary>
    public ObservableCollection<BorrowRecord> BorrowRecords { get; set; } = new();
    /// <summary>加载借阅记录</summary>
    public Task LoadBorrowRecordsAsync() => Task.CompletedTask;
    /// <summary>借书</summary>
    public Task BorrowBookAsync(BorrowRecord record) => Task.CompletedTask;
    /// <summary>还书</summary>
    public Task ReturnBookAsync(BorrowRecord record) => Task.CompletedTask;

    // ================== 统计报表 ==================
    /// <summary>加载统计报表</summary>
    public Task LoadStatisticsAsync() => Task.CompletedTask;

    // ================== 系统设置 ==================
    /// <summary>加载系统设置</summary>
    public Task LoadSettingsAsync() => Task.CompletedTask;
    /// <summary>保存系统设置</summary>
    public Task SaveSettingsAsync() => Task.CompletedTask;

    // ================== 搜索 ==================
    /// <summary>搜索关键字</summary>
    public string SearchText { get; set; } = string.Empty;
    /// <summary>执行搜索</summary>
    public Task SearchAsync(string keyword) => Task.CompletedTask;

    // ================== 登出 ==================
    /// <summary>登出命令</summary>
    public ICommand LogoutCommand { get; }

    public MainWindowViewModel()
    {
        LogoutCommand = new RelayCommand(Logout);
    }

    /// <summary>
    /// 登出方法：隐藏主窗口并显示登录窗口
    /// </summary>
    public void Logout()
    {
        Console.WriteLine("登出按钮被点击了！");
        
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            try
            {
                // 隐藏当前主窗口而不是关闭
                if (desktop.MainWindow != null)
                {
                    desktop.MainWindow.Hide();
                }
                
                // 创建并显示新的登录窗口
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                
                Console.WriteLine("登出成功：主窗口已隐藏，登录窗口已显示");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"登出过程中发生错误: {ex.Message}");
            }
        }
    }
}