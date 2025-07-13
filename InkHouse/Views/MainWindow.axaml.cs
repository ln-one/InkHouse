using Avalonia.Controls;
using InkHouse.ViewModels;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using InkHouse.Services;
using System;

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
        // 取消订阅，避免重复进入
        this.Closing -= OnWindowClosing;
        
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
            if (DataContext is IDisposable disposable)
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