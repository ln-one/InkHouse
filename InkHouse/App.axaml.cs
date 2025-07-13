using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using InkHouse.ViewModels;
using InkHouse.Views;
using InkHouse.Services;
using System;

namespace InkHouse;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        // 初始化配置
        AppConfig.Initialize();
        
        // 初始化服务容器
        ServiceManager.Initialize();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // 订阅应用程序退出事件
            desktop.Exit += OnApplicationExit;
            
            // 只显示登录窗口，不设置主窗口
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    /// <summary>
    /// 应用程序退出事件处理
    /// </summary>
    private void OnApplicationExit(object? sender, EventArgs e)
    {
        try
        {
            // 释放所有服务资源
            ServiceManager.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"应用程序退出时释放资源发生错误: {ex.Message}");
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}