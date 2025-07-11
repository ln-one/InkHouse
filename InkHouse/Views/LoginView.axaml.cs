using Avalonia.Controls;
using InkHouse.ViewModels;
using InkHouse.Models;
using InkHouse.Services;

namespace InkHouse.Views
{
    // 登录界面的代码后端
    public partial class LoginView : UserControl
    {
        // 构造函数，初始化界面和数据绑定
        public LoginView()
        {
            InitializeComponent(); // 初始化界面组件
            // 下面是数据库连接字符串，注意替换成你们自己的用户名和密码
            string connectionString = "server=47.93.254.172;port=3306;database=InternShip;user=你的用户名;password=你的密码;";
            // 创建数据库上下文工厂，用于生成数据库操作对象
            var dbFactory = new DbContextFactory(connectionString);
            // 创建用户服务，用于登录验证
            var userService = new UserService(dbFactory);
            // 实例化LoginViewModel，并传入userService，完成数据绑定
            DataContext = new LoginViewModel(userService);
        }
    }
} 