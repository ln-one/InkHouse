using Avalonia.Controls;
using InkHouse.ViewModels;
using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using InkHouse.Services;

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
                    DataContext = ServiceManager.GetService<MainWindowViewModel>()
                };
                desktop.MainWindow = mainWindow;
                mainWindow.Show();
                
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
        /// 登录窗口关闭事件处理
        /// 当用户关闭登录窗口时，退出应用程序
        /// </summary>
        private void OnLoginWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // 只做资源释放，不再调用 desktop.Shutdown()
            DisposeResources();
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
