﻿using System;
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
    
    /// <summary>加载图书列表</summary>
    [RelayCommand]
    public async Task LoadBooksAsync()
    {
        try
        {
            IsLoading = true;
            var books = await _bookService.GetBooksByTypeAsync(SelectedBookType);
            
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

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
            {
                if (desktop.MainWindow is not null) await dialog.ShowDialog(desktop.MainWindow);
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

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
            {
                if (desktop.MainWindow is not null) await dialog.ShowDialog(desktop.MainWindow);
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
                if (desktop.MainWindow is not null) await dialog.ShowDialog(desktop.MainWindow);
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
                if (desktop.MainWindow is not null) await dialog.ShowDialog(desktop.MainWindow);
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
                if (desktop.MainWindow is not null) await dialog.ShowDialog(desktop.MainWindow);
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
                if (desktop.MainWindow is not null) await dialog.ShowDialog(desktop.MainWindow);
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
    
    /// <summary>当前选中的借阅记录</summary>
    public BorrowRecord? SelectedBorrowRecord { get; set; }
    
    /// <summary>借阅搜索关键字</summary>
    public string BorrowSearchText { get; set; } = string.Empty;
    
    /// <summary>加载借阅记录</summary>
    [RelayCommand]
    public async Task LoadBorrowRecordsAsync()
    {
        try
        {
            IsLoading = true;
            var records = await _borrowRecordService.GetAllBorrowRecordsAsync();
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                BorrowRecords.Clear();
                foreach (var record in records)
                {
                    BorrowRecords.Add(record);
                }
                // 强制通知界面刷新
                OnPropertyChanged(nameof(RecentBorrowRecords));
                OnPropertyChanged(nameof(HasRecentBorrowRecords));
                Console.WriteLine($"BorrowRecords count: {BorrowRecords.Count}, 未归还: {BorrowRecords.Count(r => r.ReturnDate == null)}");
            });
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
    
    /// <summary>搜索借阅记录</summary>
    [RelayCommand]
    public async Task SearchBorrowRecordsAsync()
    {
        if (string.IsNullOrWhiteSpace(BorrowSearchText))
        {
            await LoadBorrowRecordsAsync();
            return;
        }

        try
        {
            IsLoading = true;
            var records = await _borrowRecordService.SearchBorrowRecordsAsync(BorrowSearchText);
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                BorrowRecords.Clear();
                foreach (var record in records)
                {
                    BorrowRecords.Add(record);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"搜索借阅记录失败: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    /// <summary>显示借书对话框</summary>
    [RelayCommand]
    public async Task ShowBorrowDialogAsync()
    {
        try
        {
            var borrowEditViewModel = new BorrowEditViewModel(_borrowRecordService, _bookService, _userService);
            var dialog = new BorrowEditDialog
            {
                DataContext = borrowEditViewModel
            };

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow is not null) await dialog.ShowDialog(desktop.MainWindow);
            }
            await LoadBorrowRecordsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"显示借书对话框失败: {ex.Message}");
        }
    }
    
    /// <summary>显示还书对话框</summary>
    [RelayCommand]
    public async Task ShowReturnDialogAsync()
    {
        try
        {
            var borrowEditViewModel = new BorrowEditViewModel(_borrowRecordService, _bookService, _userService);
            // 设置为还书模式
            borrowEditViewModel.IsBorrowMode = false;
            var dialog = new BorrowEditDialog
            {
                DataContext = borrowEditViewModel
            };

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow is not null) await dialog.ShowDialog(desktop.MainWindow);
            }
            await LoadBorrowRecordsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"显示还书对话框失败: {ex.Message}");
        }
    }
    
    /// <summary>还书操作</summary>
    [RelayCommand]
    public async Task ReturnBorrowRecordAsync(BorrowRecord record)
    {
        if (record == null) return;

        try
        {
            await _borrowRecordService.ReturnBookAsync(record.Id);
            await Dispatcher.UIThread.InvokeAsync(async () => await LoadBorrowRecordsAsync());
            await ShowMessageAsync("还书成功！");
        }
        catch (Exception ex)
        {
            await ShowMessageAsync($"还书失败：{ex.Message}");
        }
    }

    

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
    public async Task ShowDashboard()
    {
        Console.WriteLine("切换到仪表板视图");
        try
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var bookService = ServiceManager.GetService<BookService>();
                var userService = ServiceManager.GetService<UserService>();
                var borrowRecordService = ServiceManager.GetService<BorrowRecordService>();
                var seatService = ServiceManager.GetService<SeatService>();
                
                var dashboardViewModel = new DashboardViewModel(bookService, userService, borrowRecordService, seatService);
                var dashboardView = new DashboardView { DataContext = dashboardViewModel };
                
                Console.WriteLine($"DashboardView 创建成功: {dashboardView.GetType().Name}");
                CurrentView = dashboardView;
                SelectedMenu = "Dashboard";
                Console.WriteLine($"CurrentView 已设置为: {CurrentView?.GetType().Name}");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"创建 DashboardView 失败: {ex.Message}");
        }
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
    public async Task ShowBorrowManagement()
    {
        Console.WriteLine("切换到借阅管理视图");
        try
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var borrowManagementView = new BorrowManagementView { DataContext = this };
                Console.WriteLine($"BorrowManagementView 创建成功: {borrowManagementView.GetType().Name}");
                CurrentView = borrowManagementView;
                SelectedMenu = "BorrowManagement";
                Console.WriteLine($"CurrentView 已设置为: {CurrentView?.GetType().Name}");
                
                // 加载借阅记录数据
                await LoadBorrowRecordsAsync();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"创建 BorrowManagementView 失败: {ex.Message}");
        }
    }
    
    

    /// <summary>显示座位管理</summary>
    [RelayCommand]
    public async Task ShowSeatManagement()
    {
        var seatService = ServiceManager.GetService<SeatService>();
        var vm = new AdminSeatManagementViewModel(seatService);
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            var seatManagementView = new InkHouse.Views.AdminSeatManagementView { DataContext = vm };
            CurrentView = seatManagementView;
            SelectedMenu = "SeatManagement";
        });
    }

    // ================== 登出 ==================
    /// <summary>登出命令</summary>
    public ICommand LogoutCommand { get; }

    /// <summary>最近借阅记录（前5条）</summary>
    public IEnumerable<BorrowRecord> RecentBorrowRecords =>
        BorrowRecords
            .Where(r => r.ReturnDate == null)
            .OrderByDescending(r => r.BorrowDate)
            .Take(1);

    public bool HasRecentBorrowRecords => RecentBorrowRecords.Any();

    public string DashboardButtonClass => SelectedMenu == "Dashboard" ? "nav-item active" : "nav-item";
    public string BookManagementButtonClass => SelectedMenu == "BookManagement" ? "nav-item active" : "nav-item";
    public string UserManagementButtonClass => SelectedMenu == "UserManagement" ? "nav-item active" : "nav-item";
    public string BorrowManagementButtonClass => SelectedMenu == "BorrowManagement" ? "nav-item active" : "nav-item";
    

    [ObservableProperty]
    private List<string> _bookTypes = new();
    [ObservableProperty]
    private string _selectedBookType = "全部";
    partial void OnSelectedBookTypeChanged(string value)
    {
        _ = LoadBooksAsync();
    }

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
                await LoadBookTypesAsync();
                await LoadBooksAsync();
                await LoadBorrowRecordsAsync(); // 新增：加载借阅记录
                await ShowDashboard(); // 自动显示仪表盘
                Console.WriteLine($"MainWindowViewModel 初始化完成，Books count: {Books.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MainWindowViewModel 初始化失败: {ex.Message}");
            }
        });
        
        Console.WriteLine($"MainWindowViewModel 构造完成");
    }

    private async Task LoadBookTypesAsync()
    {
        var types = await _bookService.GetAllBookTypesAsync();
        types.Insert(0, "全部");
        BookTypes = types;
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