using Avalonia.Controls;
using InkHouse.ViewModels;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;

namespace InkHouse.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // 设置数据上下文
        DataContext = new MainWindowViewModel();
        
        // 订阅窗口关闭事件
        this.Closing += OnWindowClosing;
    }
    
    /// <summary>
    /// 窗口关闭事件处理
    /// 当用户直接关闭主窗口时，显示登录窗口
    /// </summary>
    private void OnWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // 取消默认的关闭行为
        e.Cancel = true;
        
        // 隐藏主窗口
        this.Hide();
        
        // 清除主窗口引用
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = null;
        }
        
        // 显示登录窗口
        var loginWindow = new LoginWindow();
        loginWindow.Show();
    }
}