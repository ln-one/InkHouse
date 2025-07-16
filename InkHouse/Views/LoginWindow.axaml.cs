using Avalonia.Controls;
using InkHouse.ViewModels;
using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using InkHouse.Services;
using InkHouse.Models;

namespace InkHouse.Views
{
    /// <summary>
    /// 登录窗口的代码后端
    /// 使用现有的 LoginView，支持登录成功后切换到主窗口
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// 构造函数
        /// 初始化登录窗口
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
            
            // 等待界面加载完成后订阅事件
            this.Loaded += OnWindowLoaded;
            
            // 订阅窗口关闭事件
            this.Closing += OnLoginWindowClosing;
        }

        /// <summary>
        /// 窗口加载完成事件处理
        /// </summary>
        private void OnWindowLoaded(object? sender, EventArgs e)
        {
            // 获取 LoginView 的 DataContext
            if (this.Content is LoginView loginView && loginView.DataContext is LoginViewModel loginViewModel)
            {
                // 订阅登录成功事件
                loginViewModel.LoginSuccess += OnLoginSuccess;

                // 订阅登录失败事件
                loginViewModel.LoginFailed += OnLoginFailed;

                // 订阅注册导航事件
                loginViewModel.NavigateToRegister += OnNavigateToRegister;
            }
        }

        /// <summary>
        /// 登录成功事件处理
        /// 隐藏登录窗口并显示主窗口
        /// </summary>
        private void OnLoginSuccess(Models.User user)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // 根据用户角色决定显示哪个窗口
                if (UserRoles.IsAdmin(user.Role))
                {
                    // 创建并显示主窗口
                    var mainWindow = new MainWindow
                    {
                        DataContext = ServiceManager.GetService<MainWindowViewModel>()
                    };
                    desktop.MainWindow = mainWindow;
                    mainWindow.Show();
                }
                else
                {
                    // 创建并显示用户主窗口
                    var bookService = ServiceManager.GetService<BookService>();
                    var borrowRecordService = ServiceManager.GetService<BorrowRecordService>();
                    var viewModel = new UserMainWindowViewModel(user, bookService, borrowRecordService);
                    var userMainWindow = new UserMainWindow
                    {
                        DataContext = viewModel
                    };
                    desktop.MainWindow = userMainWindow;
                    userMainWindow.Show();
                }

                // 立即隐藏登录窗口
                this.Hide();
                
                // 清理登录窗口资源
                CleanupLoginWindow();
            }
        }
        
        /// <summary>
        /// 清理登录窗口资源
        /// </summary>
        private void CleanupLoginWindow()
        {
            try
            {
                // 取消事件订阅
                if (this.Content is LoginView loginView && loginView.DataContext is LoginViewModel loginViewModel)
                {
                    loginViewModel.LoginSuccess -= OnLoginSuccess;
                    loginViewModel.LoginFailed -= OnLoginFailed;
                    loginViewModel.NavigateToRegister -= OnNavigateToRegister;
                }
                
                // 释放登录视图模型资源
                if (this.Content is LoginView view && view.DataContext is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理登录窗口资源时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 登录失败事件处理
        /// 保持登录窗口打开，显示错误信息
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        private void OnLoginFailed(string errorMessage)
        {
            // 登录失败时保持登录窗口打开
            // 错误信息已经在 LoginViewModel 中显示
            Console.WriteLine($"登录失败: {errorMessage}");
        }

        /// <summary>
        /// 导航到注册页面事件处理
        /// 关闭登录窗口并显示注册窗口
        /// </summary>
        private void OnNavigateToRegister()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // 创建并显示注册窗口
                var registerWindow = new RegisterWindow();
                registerWindow.Show();

                // 关闭当前登录窗口
                this.Close();
            }
        }
        
        /// <summary>
        /// 登录窗口关闭事件处理
        /// 当用户关闭登录窗口时，退出应用程序
        /// </summary>
        private void OnLoginWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // 取消订阅，避免重复进入
            this.Closing -= OnLoginWindowClosing;
            
            // 释放资源
            DisposeResources();
            
            // 强制退出应用程序
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        private void DisposeResources()
        {
            try
            {
                // 释放视图模型资源
                if (this.Content is LoginView loginView && loginView.DataContext is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                
                // 释放服务资源
                ServiceManager.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"释放资源时发生错误: {ex.Message}");
            }
        }
    }
}
