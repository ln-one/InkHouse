using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using InkHouse.Services;
using InkHouse.ViewModels;

namespace InkHouse.Views
{
    /// <summary>
    /// 注册窗口
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private RegisterViewModel? _viewModel;

        public RegisterWindow()
        {
            InitializeComponent();
            
            // 通过ServiceManager获取ViewModel实例
            _viewModel = ServiceManager.GetService<RegisterViewModel>();
            DataContext = _viewModel;
            
            // 订阅事件
            _viewModel.RegisterSuccess += OnRegisterSuccess;
            _viewModel.BackToLogin += OnBackToLogin;
        }

        /// <summary>
        /// 注册成功事件处理
        /// 关闭注册窗口并显示登录窗口
        /// </summary>
        private void OnRegisterSuccess(Models.User user)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // 创建并显示登录窗口
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                
                // 关闭当前注册窗口
                this.Close();
            }
        }

        /// <summary>
        /// 返回登录事件处理
        /// 关闭注册窗口并显示登录窗口
        /// </summary>
        private void OnBackToLogin()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // 创建并显示登录窗口
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                
                // 关闭当前注册窗口
                this.Close();
            }
        }

        /// <summary>
        /// 窗口关闭时清理资源
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.RegisterSuccess -= OnRegisterSuccess;
                _viewModel.BackToLogin -= OnBackToLogin;
                _viewModel.Dispose();
            }
            
            base.OnClosed(e);
        }
    }
}
