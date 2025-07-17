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
using System.Threading.Tasks;
using Avalonia.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace InkHouse;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        // 初始化配置
        // AppConfig.Initialize();
        
        // 服务容器已在Program.cs中初始化，这里不需要重复初始化
        // ServiceManager.Initialize();
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
            
            // 如果启用了自动测试，则执行测试
            if (Program.IsAutoTestEnabled)
            {
                StartAutoTest();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    /// <summary>
    /// 启动自动测试
    /// </summary>
    private void StartAutoTest()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Console.WriteLine("开始执行自动测试...");
            
            // 延迟2秒后执行，确保UI已经加载完成
            Task.Delay(2000).ContinueWith(_ => 
            {
                Dispatcher.UIThread.InvokeAsync(async () => 
                {
                    Console.WriteLine("准备测试从登录窗口导航到注册窗口...");
                    
                    // 1. 找到登录窗口
                    var loginWindow = desktop.Windows.FirstOrDefault(w => w is LoginWindow);
                    if (loginWindow == null || !(loginWindow.Content is LoginView loginView))
                    {
                        Console.WriteLine("测试失败：找不到登录窗口");
                        return;
                    }
                    
                    // 2. 获取登录窗口的ViewModel
                    var loginViewModel = loginView.DataContext as LoginViewModel;
                    if (loginViewModel == null)
                    {
                        Console.WriteLine("测试失败：找不到LoginViewModel");
                        return;
                    }
                    
                    // 3. 模拟点击注册链接
                    Console.WriteLine("模拟点击注册链接...");
                    if (loginViewModel.NavigateToRegisterCommand.CanExecute(null))
                    {
                        loginViewModel.NavigateToRegisterCommand.Execute(null);
                    }
                    
                    // 4. 等待注册窗口打开
                    await Task.Delay(1000);
                    
                    // 5. 验证注册窗口是否打开
                    var registerWindow = desktop.Windows.FirstOrDefault(w => w is RegisterWindow);
                    if (registerWindow == null)
                    {
                        Console.WriteLine("测试失败：注册窗口未打开");
                    }
                    else
                    {
                        Console.WriteLine("测试成功：注册窗口已打开");
                    }
                    
                    // 6. 等待3秒后退出测试
                    await Task.Delay(3000);
                    desktop.Shutdown();
                });
            });
        }
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

    public App()
    {
        // 服务容器已在Program.cs中初始化，这里不需要重复初始化
        // ServiceManager.Initialize();
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            Console.WriteLine("全局未捕获异常: " + e.ExceptionObject);
        };
        System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            Console.WriteLine("全局未观察到的任务异常: " + e.Exception);
            e.SetObserved();
        };
    }
}