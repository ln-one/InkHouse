using Avalonia.Controls;
using InkHouse.ViewModels;
using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

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
            }
        }

        /// <summary>
        /// 登录成功事件处理
        /// 隐藏登录窗口并显示主窗口
        /// </summary>
        private void OnLoginSuccess()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 创建并显示主窗口
            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
                desktop.MainWindow = mainWindow;
            mainWindow.Show();
            }
            
            // 隐藏登录窗口而不是关闭
            this.Hide();
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
        /// 登录窗口关闭事件处理
        /// 当用户关闭登录窗口时，退出应用程序
        /// </summary>
        private void OnLoginWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // 如果主窗口存在且可见，则隐藏主窗口
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow != null && desktop.MainWindow.IsVisible)
                {
                    desktop.MainWindow.Hide();
                }
            }
            
            // 允许登录窗口关闭，这将导致应用程序退出
            // 因为登录窗口是唯一可见的窗口
        }
    }
}
