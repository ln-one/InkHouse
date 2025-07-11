using System.Linq;
using InkHouse.Models;

namespace InkHouse.Services
{
    // 用户服务类，负责用户相关的业务逻辑
    public class UserService
    {
        // 数据库上下文工厂
        private readonly DbContextFactory _dbContextFactory;

        // 构造函数，传入数据库上下文工厂
        public UserService(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        // 登录方法，验证用户名和密码
        public User Login(string username, string password)
        {
            using var db = _dbContextFactory.CreateDbContext();
            // 密码应加密存储，这里为简化直接对比明文
            return db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
        }
    }
} 