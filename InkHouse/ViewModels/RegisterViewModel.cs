using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using InkHouse.Models;
using InkHouse.Services;

namespace InkHouse.ViewModels
{
    /// <summary>
    /// 注册界面的ViewModel
    /// 负责用户注册的数据绑定和业务逻辑
    /// </summary>
    public class RegisterViewModel : ViewModelBase
    {
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _email = string.Empty;
        private string _fullName = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
                ValidateUserName();
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                ValidatePassword();
                ValidateConfirmPassword(); // 重新验证确认密码
            }
        }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                ValidateConfirmPassword();
            }
        }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                ValidateEmail();
            }
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set
            {
                SetProperty(ref _fullName, value);
                ValidateFullName();
            }
        }

        // 验证错误消息
        private string _userNameError = string.Empty;
        private string _passwordError = string.Empty;
        private string _confirmPasswordError = string.Empty;
        private string _emailError = string.Empty;
        private string _fullNameError = string.Empty;

        public string UserNameError
        {
            get => _userNameError;
            set => SetProperty(ref _userNameError, value);
        }

        public string PasswordError
        {
            get => _passwordError;
            set => SetProperty(ref _passwordError, value);
        }

        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set => SetProperty(ref _confirmPasswordError, value);
        }

        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }

        public string FullNameError
        {
            get => _fullNameError;
            set => SetProperty(ref _fullNameError, value);
        }

        /// <summary>
        /// 注册成功事件
        /// </summary>
        public event Action<User>? RegisterSuccess;

        /// <summary>
        /// 返回登录事件
        /// </summary>
        public event Action? BackToLogin;

        /// <summary>
        /// 注册命令
        /// </summary>
        public ICommand RegisterCommand { get; }

        /// <summary>
        /// 返回登录命令
        /// </summary>
        public ICommand BackToLoginCommand { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RegisterViewModel()
        {
            RegisterCommand = new AsyncRelayCommand(RegisterAsync, CanRegister);
            BackToLoginCommand = new RelayCommand(() => BackToLogin?.Invoke());
        }

        /// <summary>
        /// 检查是否可以注册
        /// </summary>
        private bool CanRegister()
        {
            return !IsLoading &&
                   !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(FullName) &&
                   string.IsNullOrEmpty(UserNameError) &&
                   string.IsNullOrEmpty(PasswordError) &&
                   string.IsNullOrEmpty(ConfirmPasswordError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(FullNameError);
        }

        /// <summary>
        /// 异步注册方法
        /// </summary>
        private async Task RegisterAsync()
        {
            await ExecuteAsync(async () =>
            {
                // 最终验证
                if (!ValidateAllFields())
                {
                    ShowError("请检查输入信息");
                    return;
                }

                // 使用ServiceManager获取用户服务
                var userService = ServiceManager.GetService<UserService>();
                
                // 创建新用户对象
                var newUser = new User
                {
                    UserName = UserName.Trim(),
                    Password = Password, // 密码将在服务层加密
                    Role = UserRoles.User // 注册用户默认为普通用户
                };

                // 调用注册服务
                var result = await userService.RegisterAsync(newUser, Email.Trim(), FullName.Trim());
                
                if (result.Success)
                {
                    ShowSuccess("注册成功！正在跳转到登录页面...");
                    
                    // 延迟1秒后触发注册成功事件
                    await Task.Delay(1000);
                    RegisterSuccess?.Invoke(result.User!);
                }
                else
                {
                    ShowError(result.ErrorMessage ?? "注册失败，请重试");
                }
            });
        }

        #region 验证方法

        /// <summary>
        /// 验证用户名
        /// </summary>
        private void ValidateUserName()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                UserNameError = "用户名不能为空";
                return;
            }

            if (UserName.Length < 3)
            {
                UserNameError = "用户名至少需要3个字符";
                return;
            }

            if (UserName.Length > 20)
            {
                UserNameError = "用户名不能超过20个字符";
                return;
            }

            // 检查用户名格式：只允许字母、数字和下划线
            if (!Regex.IsMatch(UserName, @"^[a-zA-Z0-9_]+$"))
            {
                UserNameError = "用户名只能包含字母、数字和下划线";
                return;
            }

            UserNameError = string.Empty;
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "密码不能为空";
                return;
            }

            if (Password.Length < 6)
            {
                PasswordError = "密码至少需要6个字符";
                return;
            }

            if (Password.Length > 50)
            {
                PasswordError = "密码不能超过50个字符";
                return;
            }

            // 检查密码强度：至少包含字母和数字
            bool hasLetter = Regex.IsMatch(Password, @"[a-zA-Z]");
            bool hasDigit = Regex.IsMatch(Password, @"\d");

            if (!hasLetter || !hasDigit)
            {
                PasswordError = "密码必须包含至少一个字母和一个数字";
                return;
            }

            PasswordError = string.Empty;
        }

        /// <summary>
        /// 验证确认密码
        /// </summary>
        private void ValidateConfirmPassword()
        {
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ConfirmPasswordError = "请确认密码";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ConfirmPasswordError = "两次输入的密码不一致";
                return;
            }

            ConfirmPasswordError = string.Empty;
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "邮箱地址不能为空";
                return;
            }

            // 邮箱格式验证
            var emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(Email, emailRegex))
            {
                EmailError = "请输入有效的邮箱地址";
                return;
            }

            EmailError = string.Empty;
        }

        /// <summary>
        /// 验证真实姓名
        /// </summary>
        private void ValidateFullName()
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                FullNameError = "真实姓名不能为空";
                return;
            }

            if (FullName.Trim().Length < 2)
            {
                FullNameError = "真实姓名至少需要2个字符";
                return;
            }

            if (FullName.Length > 20)
            {
                FullNameError = "真实姓名不能超过20个字符";
                return;
            }

            FullNameError = string.Empty;
        }

        /// <summary>
        /// 验证所有字段
        /// </summary>
        private bool ValidateAllFields()
        {
            ValidateUserName();
            ValidatePassword();
            ValidateConfirmPassword();
            ValidateEmail();
            ValidateFullName();

            return string.IsNullOrEmpty(UserNameError) &&
                   string.IsNullOrEmpty(PasswordError) &&
                   string.IsNullOrEmpty(ConfirmPasswordError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(FullNameError);
        }

        #endregion
    }
}
