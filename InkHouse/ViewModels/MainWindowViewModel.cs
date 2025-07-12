using Avalonia.Controls;

namespace InkHouse.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private Control _currentView;

    /// <summary>
    /// 当前显示的视图
    /// </summary>
    public Control CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    public MainWindowViewModel()
    {
        // 默认显示登录界面
        CurrentView = new Views.LoginView();
    }
}