using Avalonia.Controls;
using InkHouse.ViewModels;
using InkHouse.Services;

namespace InkHouse.Views
{
    /// <summary>
    /// 登录界面的代码后端
    /// 使用新的架构，无需手动创建服务和数据库连接
    /// </summary>
    public partial class LoginView : UserControl
    {
        /// <summary>
        /// 构造函数
        /// 初始化界面和数据绑定，使用新的架构简化代码
        /// </summary>
        public LoginView()
        {
            InitializeComponent(); // 初始化界面组件
            
            // 使用ServiceManager获取LoginViewModel实例
            DataContext = ServiceManager.GetService<LoginViewModel>();
        }
    }
} 