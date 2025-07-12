using Avalonia.Controls;
using InkHouse.ViewModels;

namespace InkHouse.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}