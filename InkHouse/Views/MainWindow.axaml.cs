using Avalonia.Controls;
using InkHouse.ViewModels;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using InkHouse.Services;

namespace InkHouse.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // 使用ServiceManager获取MainWindowViewModel实例
        DataContext = ServiceManager.GetService<MainWindowViewModel>();
        
        // 订阅窗口关闭事件
        this.Closing += OnWindowClosing;
    }
    
    /// <summary>
    /// 窗口关闭事件处理
    /// 当用户直接关闭主窗口时，退出应用程序
    /// </summary>
    private void OnWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // 允许窗口正常关闭，这将导致应用程序退出
        // 不取消关闭行为，让应用程序正常退出
    }
}