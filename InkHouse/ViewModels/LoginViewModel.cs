using System;
using System.Threading.Tasks;
using System.Windows.Input;
using InkHouse.Models;
using InkHouse.Services;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace InkHouse.ViewModels
{
    /// <summary>
    /// 登录界面的ViewModel
    /// 负责数据绑定和登录逻辑，使用新的架构简化开发
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private string _userName;
        private string _password;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// 登录命令
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// 构造函数
        /// 使用ServiceManager获取服务，无需手动创建
        /// </summary>
        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        /// <summary>
        /// 导航到主界面
        /// </summary>
        /// <param name="user">登录成功的用户</param>
        private void NavigateToMainView(User user)
        {
            Console.WriteLine("开始导航到主界面");
            
            // 通过主窗口的ViewModel来切换视图
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Console.WriteLine("获取到桌面应用生命周期");
                var mainWindow = desktop.MainWindow;
                Console.WriteLine($"主窗口: {mainWindow}");
                
                if (mainWindow?.DataContext is MainWindowViewModel mainWindowViewModel)
                {
                    Console.WriteLine("获取到主窗口ViewModel");
                    var mainView = new Views.MainView
                    {
                        DataContext = new MainViewModel(user)
                    };
                    Console.WriteLine("创建主界面视图成功");
                    mainWindowViewModel.CurrentView = mainView;
                    Console.WriteLine("设置主界面视图成功");
                }
                else
                {
                    Console.WriteLine("主窗口DataContext不是MainWindowViewModel类型");
                }
            }
            else
            {
                Console.WriteLine("无法获取桌面应用生命周期");
            }
        }

        /// <summary>
        /// 异步登录方法
        /// 使用新的架构，自动处理错误和加载状态
        /// </summary>
        private async Task LoginAsync()
        {
            await ExecuteAsync(async () =>
            {
                // 输入验证
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    ShowError("请输入用户名");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    ShowError("请输入密码");
                    return;
                }

                // 使用ServiceManager获取用户服务
                var userService = ServiceManager.Instance.UserService;
                
                // 先尝试测试登录，如果失败再尝试真实数据库登录
                var user = await userService.TestLoginAsync(UserName, Password);
                if (user == null)
                {
                    // 如果测试登录失败，尝试真实数据库登录
                    user = await userService.LoginAsync(UserName, Password);
                }

                if (user != null)
                {
                    ShowSuccess($"欢迎回来，{user.UserName}！");
                    // 导航到主界面
                    NavigateToMainView(user);
                }
                else
                {
                    ShowError("用户名或密码错误，请重试");
                }
            });
        }
    }
} 