using Avalonia.Controls;
using InkHouse.ViewModels;

namespace InkHouse.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // 设置数据上下文
        DataContext = new MainWindowViewModel();
    }
}