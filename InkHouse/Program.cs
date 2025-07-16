using Avalonia;
using System;
using InkHouse.Services;

namespace InkHouse;

sealed class Program
{
    // 是否启用自动测试
    public static bool IsAutoTestEnabled = false;
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        ServiceManager.Initialize();
        // 检查是否启用自动测试
        if (args.Length > 0 && args[0] == "--auto-test")
        {
            Console.WriteLine("自动测试模式已启用");
            IsAutoTestEnabled = true;
        }
        
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}