using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using InkHouse.Models;
using InkHouse.Services;

namespace InkHouse.ViewModels
{
    // 登录界面的ViewModel，负责数据绑定和登录逻辑
    public class LoginViewModel : INotifyPropertyChanged
    {
        // 用户名字段
        private string _userName;
        // 密码字段
        private string _password;
        // 用户服务，用于登录验证
        private readonly UserService _userService;
        // 属性变更事件，支持数据绑定
        public event PropertyChangedEventHandler PropertyChanged;

        // 用户名属性，支持双向绑定
        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }
        // 密码属性，支持双向绑定
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        // 登录命令，绑定到登录按钮
        public ICommand LoginCommand { get; }

        // 构造函数，传入UserService
        public LoginViewModel(UserService userService)
        {
            _userService = userService;
            LoginCommand = new RelayCommand(Login);
        }

        // 登录方法，执行登录逻辑
        private void Login()
        {
            var user = _userService.Login(UserName, Password);
            if (user != null)
            {
                // 登录成功，后续可导航到主界面
            }
            else
            {
                // 登录失败，提示用户
            }
        }

        // 属性变更通知方法
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // 命令实现类，用于绑定按钮点击事件
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute();
    }
} 