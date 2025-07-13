using Avalonia.Controls;
using Avalonia.Interactivity;
using InkHouse.Models;
using InkHouse.ViewModels;

namespace InkHouse.Views
{
    public partial class UserManagementView : UserControl
    {
        public UserManagementView()
        {
            InitializeComponent();
        }

        private void EditUser_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is User user && DataContext is MainWindowViewModel vm)
            {
                vm.SelectedUser = user;
                vm.EditUserCommand.Execute(null);
            }
        }

        private void DeleteUser_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is User user && DataContext is MainWindowViewModel vm)
            {
                vm.SelectedUser = user;
                vm.DeleteUserCommand.Execute(null);
            }
        }
    }
} 