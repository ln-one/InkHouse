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

namespace InkHouse.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly BookService _bookService;
    private readonly UserService _userService;
    private readonly BorrowRecordService _borrowRecordService;

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
    
    /// <summary>当前选中的图书</summary>
    public Book? SelectedBook { get; set; }
    
    /// <summary>搜索关键字</summary>
    public string BookSearchText { get; set; } = string.Empty;
    
    /// <summary>是否正在加载</summary>
    public new bool IsLoading { get; set; }
    
    /// <summary>加载图书列表</summary>
    [RelayCommand]
    public async Task LoadBooksAsync()
    {
        try
        {
            IsLoading = true;
            var books = await _bookService.GetAllBooksAsync();
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Books.Clear();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            });
        }
        catch (Exception ex)
        {
            // TODO: 显示错误消息
            Console.WriteLine($"加载图书失败: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    /// <summary>搜索图书</summary>
    [RelayCommand]
    public async Task SearchBooksAsync()
    {
        if (string.IsNullOrWhiteSpace(BookSearchText))
        {
            await LoadBooksAsync();
            return;
        }

        try
        {
            IsLoading = true;
            var books = await _bookService.SearchBooksAsync(BookSearchText);
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Books.Clear();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            });
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
    
    /// <summary>添加图书</summary>
    [RelayCommand]
    public async Task AddBookAsync()
    {
        try
        {
            var bookEditViewModel = new BookEditViewModel(_bookService);
            var dialog = new BookEditDialog
            {
                DataContext = bookEditViewModel
            };

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await dialog.ShowDialog(desktop.MainWindow);
            }
            await LoadBooksAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"添加图书失败: {ex.Message}");
        }
    }
    
    /// <summary>编辑图书</summary>
    [RelayCommand]
    public async Task EditBookAsync()
    {
        if (SelectedBook == null) return;

        try
        {
            var bookEditViewModel = new BookEditViewModel(_bookService, SelectedBook);
            var dialog = new BookEditDialog
            {
                DataContext = bookEditViewModel
            };

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await dialog.ShowDialog(desktop.MainWindow);
            }
            await LoadBooksAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"编辑图书失败: {ex.Message}");
        }
    }
    
    /// <summary>删除图书</summary>
    [RelayCommand]
    public async Task DeleteBookAsync()
    {
        if (SelectedBook == null)
        {
            await ShowMessageAsync("请先选中要删除的图书！");
            return;
        }

        // 弹窗确认
        var confirm = await ShowConfirmAsync($"确定要删除《{SelectedBook.Title}》吗？");
        if (!confirm) return;

        try
        {
            await _bookService.DeleteBookAsync(SelectedBook.Id);
            await Dispatcher.UIThread.InvokeAsync(async () => await LoadBooksAsync());
            await ShowMessageAsync("删除成功！");
        }
        catch (Exception ex)
        {
            await ShowMessageAsync($"删除失败：{ex.Message}");
        }
    }

    // 简单的消息弹窗
    private async Task ShowMessageAsync(string message)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
            {
                Window? dialog = null;
                dialog = new Window
                {
                    Title = "提示",
                    Width = 300,
                    Height = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Content = new StackPanel
                    {
                        Children =
                        {
                            new TextBlock { Text = message, Margin = new Thickness(20), TextWrapping = TextWrapping.Wrap },
                            new Button { Content = "确定", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, Margin = new Thickness(0,10,0,0), Command = new RelayCommand(() => dialog?.Close()) }
                        }
                    }
                };
                await dialog.ShowDialog(desktop.MainWindow);
            }
        });
    }

    // 简单的确认弹窗
    private async Task<bool> ShowConfirmAsync(string message)
    {
        return await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
            {
                var tcs = new TaskCompletionSource<bool>();
                Window? dialog = null;
                dialog = new Window
                {
                    Title = "确认",
                    Width = 300,
                    Height = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Content = new StackPanel
                    {
                        Children =
                        {
                            new TextBlock { Text = message, Margin = new Thickness(20), TextWrapping = TextWrapping.Wrap },
                            new StackPanel
                            {
                                Orientation = Avalonia.Layout.Orientation.Horizontal,
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                Spacing = 16,
                                Children =
                                {
                                    new Button { Content = "取消", Command = new RelayCommand(() => { tcs.SetResult(false); dialog?.Close(); }) },
                                    new Button { Content = "确定", Command = new RelayCommand(() => { tcs.SetResult(true); dialog?.Close(); }) }
                                }
                            }
                        }
                    }
                };
                await dialog.ShowDialog(desktop.MainWindow);
                return await tcs.Task;
            }
            return false;
        });
    }

    // ================== 用户管理 ==================
    /// <summary>用户列表</summary>
    public ObservableCollection<User> Users { get; set; } = new();
    
    /// <summary>当前选中的用户</summary>
    public User? SelectedUser { get; set; }
    
    /// <summary>用户搜索关键字</summary>
    public string UserSearchText { get; set; } = string.Empty;
    
    /// <summary>加载用户列表</summary>
    [RelayCommand]
    public async Task LoadUsersAsync()
    {
        try
        {
            IsLoading = true;
            var users = await _userService.GetAllUsersAsync();
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加载用户失败: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    /// <summary>搜索用户</summary>
    [RelayCommand]
    public async Task SearchUsersAsync()
    {
        if (string.IsNullOrWhiteSpace(UserSearchText))
        {
            await LoadUsersAsync();
            return;
        }

        try
        {
            IsLoading = true;
            var users = await _userService.SearchUsersAsync(UserSearchText);
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"搜索用户失败: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    /// <summary>添加用户</summary>
    [RelayCommand]
    public async Task AddUserAsync()
    {
        try
        {
            var userEditViewModel = new UserEditViewModel(_userService);
            var dialog = new UserEditDialog
            {
                DataContext = userEditViewModel
            };

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await dialog.ShowDialog(desktop.MainWindow);
            }
            await LoadUsersAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"添加用户失败: {ex.Message}");
        }
    }
    
    /// <summary>编辑用户</summary>
    [RelayCommand]
    public async Task EditUserAsync()
    {
        Console.WriteLine($"EditUserAsync called, SelectedUser: {(SelectedUser == null ? "null" : SelectedUser.UserName)}");
        if (SelectedUser == null) return;
        try
        {
            Console.WriteLine("准备弹出编辑用户对话框");
            var userEditViewModel = new UserEditViewModel(_userService, SelectedUser);
            var dialog = new UserEditDialog
            {
                DataContext = userEditViewModel
            };
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await dialog.ShowDialog(desktop.MainWindow);
                Console.WriteLine("编辑用户对话框已关闭");
            }
            await LoadUsersAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"编辑用户异常: {ex}");
        }
    }
    
    /// <summary>删除用户</summary>
    [RelayCommand]
    public async Task DeleteUserAsync()
    {
        Console.WriteLine($"DeleteUserAsync called, SelectedUser: {(SelectedUser == null ? "null" : SelectedUser.UserName)}");
        if (SelectedUser == null)
        {
            await ShowMessageAsync("请先选中要删除的用户！");
            return;
        }
        // 弹窗确认
        var confirm = await ShowConfirmAsync($"确定要删除用户 '{SelectedUser.UserName}' 吗？");
        if (!confirm) return;
        try
        {
            await _userService.DeleteUserAsync(SelectedUser.Id);
            await Dispatcher.UIThread.InvokeAsync(async () => await LoadUsersAsync());
            await ShowMessageAsync("删除成功！");
        }
        catch (Exception ex)
        {
            await ShowMessageAsync($"删除失败：{ex.Message}");
        }
    }

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
    [RelayCommand]
    public async Task LoadStatisticsAsync()
    {
        try
        {
            var (totalBooks, availableBooks, borrowedBooks) = await _bookService.GetBookStatisticsAsync();
            TotalBooks = totalBooks;
            AvailableBooks = availableBooks;
            BorrowedBooks = borrowedBooks;
            
            // 加载用户统计
            var users = await _userService.GetAllUsersAsync();
            RegisteredUsers = users.Count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加载统计信息失败: {ex.Message}");
        }
    }

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

    // ================== 视图管理 ==================
    /// <summary>当前视图</summary>
    [ObservableProperty]
    private object _currentView = "Dashboard";
    
    // ================== 导航命令 ==================
    /// <summary>当前选中的菜单项</summary>
    [ObservableProperty]
    private string _selectedMenu = "Dashboard";
    
    /// <summary>显示仪表板</summary>
    [RelayCommand]
    public void ShowDashboard()
    {
        SelectedMenu = "Dashboard";
        CurrentView = "Dashboard";
    }
    
    /// <summary>显示图书管理</summary>
    [RelayCommand]
    public async Task ShowBookManagement()
    {
        Console.WriteLine("切换到图书管理视图");
        try
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var bookManagementView = new BookManagementView { DataContext = this };
                Console.WriteLine($"BookManagementView 创建成功: {bookManagementView.GetType().Name}");
                CurrentView = bookManagementView;
                SelectedMenu = "BookManagement";
                Console.WriteLine($"CurrentView 已设置为: {CurrentView?.GetType().Name}");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"创建 BookManagementView 失败: {ex.Message}");
        }
    }
    
    /// <summary>显示用户管理</summary>
    [RelayCommand]
    public async Task ShowUserManagement()
    {
        Console.WriteLine("切换到用户管理视图");
        try
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var userManagementView = new UserManagementView { DataContext = this };
                Console.WriteLine($"UserManagementView 创建成功: {userManagementView.GetType().Name}");
                CurrentView = userManagementView;
                SelectedMenu = "UserManagement";
                Console.WriteLine($"CurrentView 已设置为: {CurrentView?.GetType().Name}");
                
                // 加载用户数据
                await LoadUsersAsync();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"创建 UserManagementView 失败: {ex.Message}");
        }
    }
    
    /// <summary>显示借阅管理</summary>
    [RelayCommand]
    public void ShowBorrowManagement()
    {
        SelectedMenu = "BorrowManagement";
        CurrentView = "BorrowManagement";
    }
    
    /// <summary>显示统计报表</summary>
    [RelayCommand]
    public void ShowStatistics()
    {
        SelectedMenu = "Statistics";
        CurrentView = "Statistics";
    }
    
    /// <summary>显示系统设置</summary>
    [RelayCommand]
    public void ShowSettings()
    {
        SelectedMenu = "Settings";
        CurrentView = "Settings";
    }

    // ================== 登出 ==================
    /// <summary>登出命令</summary>
    public ICommand LogoutCommand { get; }

    public string DashboardButtonClass => SelectedMenu == "Dashboard" ? "nav-item active" : "nav-item";
    public string BookManagementButtonClass => SelectedMenu == "BookManagement" ? "nav-item active" : "nav-item";
    public string UserManagementButtonClass => SelectedMenu == "UserManagement" ? "nav-item active" : "nav-item";
    public string BorrowManagementButtonClass => SelectedMenu == "BorrowManagement" ? "nav-item active" : "nav-item";
    public string StatisticsButtonClass => SelectedMenu == "Statistics" ? "nav-item active" : "nav-item";
    public string SettingsButtonClass => SelectedMenu == "Settings" ? "nav-item active" : "nav-item";

    public MainWindowViewModel(BookService bookService, UserService userService, BorrowRecordService borrowRecordService)
    {
        _bookService = bookService;
        _userService = userService;
        _borrowRecordService = borrowRecordService;
        
        LogoutCommand = new RelayCommand(Logout);
        
        // 延迟初始化数据，避免在构造函数中进行异步操作
        Dispatcher.UIThread.Post(async () =>
        {
            try
            {
                await LoadStatisticsAsync();
                await LoadBooksAsync();
                Console.WriteLine($"MainWindowViewModel 初始化完成，Books count: {Books.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MainWindowViewModel 初始化失败: {ex.Message}");
            }
        });
        
        Console.WriteLine($"MainWindowViewModel 构造完成");
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