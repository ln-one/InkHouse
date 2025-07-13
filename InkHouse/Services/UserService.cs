using System;
using System.Collections.Generic;
using System.Linq;
using InkHouse.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InkHouse.Services
{
    /// <summary>
    /// 用户服务类
    /// 负责用户相关的业务逻辑操作
    /// </summary>
    public class UserService
    {
        private readonly DbContextFactory _dbContextFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextFactory">数据库上下文工厂</param>
        public UserService(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录成功的用户对象，失败返回null</returns>
        public User? Login(string username, string password)
        {
            try
            {
                Console.WriteLine("开始登录验证...");
                using var db = _dbContextFactory.CreateDbContext();
                // 注意：在实际项目中，密码应该加密存储和验证
                var user = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
                Console.WriteLine($"查询结果: {(user != null ? "找到用户" : "未找到用户")}");
        
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"用户登录时发生错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 异步用户登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录成功的用户对象，失败返回null</returns>
        public async Task<User?> LoginAsync(string username, string password)
        {
            try
            {
                Console.WriteLine($"尝试登录用户: {username}");
                using var db = _dbContextFactory.CreateDbContext();
                Console.WriteLine("数据库上下文创建成功");
                
                // 注意：在实际项目中，密码应该加密存储和验证
                var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
                
                if (user != null)
                {
                    Console.WriteLine($"用户登录成功: {user.UserName}");
                }
                else
                {
                    Console.WriteLine("用户登录失败：用户名或密码错误");
                }
                
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"用户登录时发生错误: {ex.Message}");
                Console.WriteLine($"错误详情: {ex}");
                return null;
            }
        }

        /// <summary>
        /// 临时测试登录方法（用于测试界面切换）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>测试用户对象</returns>
        public async Task<User?> TestLoginAsync(string username, string password)
        {
            // 模拟网络延迟
            await Task.Delay(500);
            
            // 简单的测试用户验证
            if (username == "admin" && password == "123456")
            {
                Console.WriteLine("使用测试登录成功");
                return new User
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "123456",
                    Role = "管理员"
                };
            }
            
            Console.WriteLine("测试登录失败：用户名或密码错误");
            return null;
        }

        // TODO: 在这里添加其他用户相关的业务逻辑方法
        // 例如：GetAllUsers(), AddUser(), UpdateUser(), DeleteUser() 等
    }
} 