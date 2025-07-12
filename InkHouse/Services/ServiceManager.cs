using InkHouse.Models;

namespace InkHouse.Services
{
    /// <summary>
    /// 服务管理器类
    /// 统一管理所有服务实例，避免重复创建
    /// 使用单例模式确保全局只有一个实例
    /// </summary>
    public class ServiceManager
    {
        // 单例实例
        private static ServiceManager _instance;
        private static readonly object _lock = new object();

        // 数据库上下文工厂
        private readonly DbContextFactory _dbContextFactory;
        
        // 各种服务实例
        private readonly UserService _userService;
        private readonly BookService _bookService;
        private readonly BorrowRecordService _borrowRecordService;

        /// <summary>
        /// 私有构造函数，防止外部直接创建实例
        /// </summary>
        private ServiceManager()
        {
            // 使用配置中的连接字符串创建数据库上下文工厂
            _dbContextFactory = new DbContextFactory(AppConfig.DatabaseConnectionString);
            
            // 初始化各种服务
            _userService = new UserService(_dbContextFactory);
            _bookService = new BookService(_dbContextFactory);
            _borrowRecordService = new BorrowRecordService(_dbContextFactory);
        }

        /// <summary>
        /// 获取ServiceManager的单例实例
        /// </summary>
        public static ServiceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServiceManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 获取用户服务实例
        /// </summary>
        public UserService UserService => _userService;

        /// <summary>
        /// 获取图书服务实例
        /// </summary>
        public BookService BookService => _bookService;

        /// <summary>
        /// 获取借阅记录服务实例
        /// </summary>
        public BorrowRecordService BorrowRecordService => _borrowRecordService;

        /// <summary>
        /// 获取数据库上下文工厂
        /// </summary>
        public DbContextFactory DbContextFactory => _dbContextFactory;

        /// <summary>
        /// 重新初始化服务（当配置更改时调用）
        /// </summary>
        public void Reinitialize()
        {
            lock (_lock)
            {
                _instance = new ServiceManager();
            }
        }
    }
} 