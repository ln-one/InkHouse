using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InkHouse.Models;
using InkHouse.Services;
using InkHouse.Views;
using System.Collections.Generic;

namespace InkHouse.ViewModels
{
    public partial class UserEditViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private readonly User? _originalUser;

        [ObservableProperty]
        private string _userName = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private string _role = "User";

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private bool _isEditMode = false;

        public List<string> RoleOptions { get; } = new() { "User", "Admin" };

        public UserEditViewModel(UserService userService, User? user = null)
        {
            _userService = userService;
            _originalUser = user;

            if (user != null)
            {
                // 编辑模式：填充现有数据
                IsEditMode = true;
                UserName = user.UserName;
                Role = user.Role;
                // 注意：出于安全考虑，编辑模式下不显示原密码
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            try
            {
                IsLoading = true;

                // 验证必填字段
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    // TODO: 显示错误消息
                    return;
                }

                if (!IsEditMode && string.IsNullOrWhiteSpace(Password))
                {
                    // TODO: 显示错误消息
                    return;
                }

                if (!IsEditMode && Password != ConfirmPassword)
                {
                    // TODO: 显示错误消息
                    return;
                }

                if (string.IsNullOrWhiteSpace(Role))
                {
                    // TODO: 显示错误消息
                    return;
                }

                var user = new User
                {
                    UserName = UserName,
                    Password = IsEditMode ? _originalUser!.Password : Password, // 编辑模式下保持原密码
                    Role = Role
                };

                if (_originalUser != null)
                {
                    // 编辑模式
                    user.Id = _originalUser.Id;
                    await _userService.UpdateUserAsync(user);
                }
                else
                {
                    // 新增模式
                    await _userService.AddUserAsync(user);
                }

                // 关闭对话框
                if (App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
                {
                    foreach (var window in desktop.Windows)
                    {
                        if (window is UserEditDialog dialog)
                        {
                            dialog.Close();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: 显示错误消息
                Console.WriteLine($"保存用户失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            // 关闭对话框
            if (App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (var window in desktop.Windows)
                {
                    if (window is UserEditDialog dialog)
                    {
                        dialog.Close();
                        break;
                    }
                }
            }
        }
    }
} 