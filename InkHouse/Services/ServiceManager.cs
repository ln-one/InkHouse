using Microsoft.Extensions.DependencyInjection;
using InkHouse.Models;
using InkHouse.Services;
using InkHouse.ViewModels;
using System;

namespace InkHouse.Services
{
    /// <summary>
    /// 服务管理器，负责管理所有服务的依赖注入
    /// </summary>
    public static class ServiceManager
    {
        private static IServiceProvider? _serviceProvider;

        /// <summary>
        /// 初始化服务容器
        /// </summary>
        public static void Initialize()
        {
            var services = new ServiceCollection();

            // 注册数据库上下文工厂
            services.AddSingleton<DbContextFactory>();

            // 注册业务服务
            services.AddSingleton<BookService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<BorrowRecordService>();
            services.AddSingleton<SeatService>();

            // 注册视图模型
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<BookEditViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("服务容器未初始化，请先调用 Initialize() 方法");
            }

            return _serviceProvider.GetRequiredService<T>();
        }
        
        /// <summary>
        /// 释放服务资源
        /// </summary>
        public static void Dispose()
        {
            try
            {
                if (_serviceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                    _serviceProvider = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"释放服务资源时发生错误: {ex.Message}");
            }
        }
    }
} 