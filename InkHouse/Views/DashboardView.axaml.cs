using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InkHouse.ViewModels;
using InkHouse.Services;

namespace InkHouse.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            
            // 如果没有设置DataContext，则创建一个新的DashboardViewModel
            if (DataContext == null)
            {
                var bookService = ServiceManager.GetService<BookService>();
                var userService = ServiceManager.GetService<UserService>();
                var borrowRecordService = ServiceManager.GetService<BorrowRecordService>();
                var seatService = ServiceManager.GetService<SeatService>();
                
                DataContext = new DashboardViewModel(bookService, userService, borrowRecordService, seatService);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}