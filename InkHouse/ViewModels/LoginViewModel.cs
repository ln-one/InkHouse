using System;
using System.Threading.Tasks;
using System.Windows.Input;
using InkHouse.Models;
using InkHouse.Services;
using CommunityToolkit.Mvvm.Input;

namespace InkHouse.ViewModels
{
    /// <summary>
    /// 登录界面的ViewModel
    /// 负责数据绑定和登录逻辑，使用新的架构简化开发
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private string _userName = string.Empty;
        private string _password = string.Empty;

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
        /// 登录成功事件
        /// </summary>
        public event Action<User>? LoginSuccess;
        
        /// <summary>
        /// 登录失败事件（权限不足）
        /// </summary>
        public event Action<string>? LoginFailed;

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
        /// 异步登录方法
        /// 使用新的架构，自动处理错误和加载状态
        /// </summary>
        private async Task LoginAsync()
        {   
            Console.WriteLine("登录按钮被点击了！");

            await ExecuteAsync(async () =>
            {
                Console.WriteLine($"用户名: {UserName}, 密码: {Password}");
                
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
                var userService = ServiceManager.GetService<UserService>();
                var user = await Task.Run(() => userService.Login(UserName, Password));

                if (user != null)
                {
                    ShowSuccess($"欢迎回来，{user.UserName}！");
                        
                    // 触发登录成功事件
                    LoginSuccess?.Invoke(user);
                }
                else
                {
                    ShowError("用户名或密码错误，请重试");
                    
                    // 触发登录失败事件（凭据错误）
                    LoginFailed?.Invoke("用户名或密码错误，请重试");
                }
            });
        }
    }
} 