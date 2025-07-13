using Avalonia.Controls;

namespace InkHouse.Views
{
    public partial class BorrowEditDialog : Window
    {
        public BorrowEditDialog()
        {
            InitializeComponent();
            
            // 绑定取消按钮事件
            if (this.FindControl<Button>("CancelButton") is Button cancelButton)
            {
                cancelButton.Click += (sender, e) => this.Close();
            }
        }
    }
} 