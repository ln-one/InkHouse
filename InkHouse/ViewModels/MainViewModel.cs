using System;
using System.Windows.Input;
using InkHouse.Models;
using CommunityToolkit.Mvvm.Input;

namespace InkHouse.ViewModels
{
    /// <summary>
    /// 主界面视图模型
    /// 负责主界面的数据绑定和导航逻辑
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        private User _currentUser = null!;
        private bool _isBorrowManagementSelected = true;
        private bool _isSeatReservationSelected = false;
        private object _currentView = null!;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        /// <summary>
        /// 是否选中借阅管理
        /// </summary>
        public bool IsBorrowManagementSelected
        {
            get => _isBorrowManagementSelected;
            set => SetProperty(ref _isBorrowManagementSelected, value);
        }

        /// <summary>
        /// 是否选中座位预约
        /// </summary>
        public bool IsSeatReservationSelected
        {
            get => _isSeatReservationSelected;
            set => SetProperty(ref _isSeatReservationSelected, value);
        }

        /// <summary>
        /// 当前显示的内容视图
        /// </summary>
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        /// <summary>
        /// 选择借阅管理命令
        /// </summary>
        public ICommand SelectBorrowManagementCommand { get; }

        /// <summary>
        /// 选择座位预约命令
        /// </summary>
        public ICommand SelectSeatReservationCommand { get; }

        /// <summary>
        /// 退出登录命令
        /// </summary>
        public ICommand LogoutCommand { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user">当前登录用户</param>
        public MainViewModel(User user)
        {
            CurrentUser = user;
            SelectBorrowManagementCommand = new RelayCommand(SelectBorrowManagement);
            SelectSeatReservationCommand = new RelayCommand(SelectSeatReservation);
            LogoutCommand = new RelayCommand(Logout);
            
            // 默认显示借阅管理
            IsBorrowManagementSelected = true;
            IsSeatReservationSelected = false;
        }

        /// <summary>
        /// 选择借阅管理
        /// </summary>
        private void SelectBorrowManagement()
        {
            IsBorrowManagementSelected = true;
            IsSeatReservationSelected = false;
        }

        /// <summary>
        /// 选择座位预约
        /// </summary>
        private void SelectSeatReservation()
        {
            IsBorrowManagementSelected = false;
            IsSeatReservationSelected = true;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        private void Logout()
        {
            // 这里可以添加退出登录的逻辑
            // 例如清除用户会话、导航回登录界面等
            ShowSuccess("已退出登录");
        }
    }
} 