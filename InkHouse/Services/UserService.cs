using System;
using System.Collections.Generic;
using System.Linq;
using InkHouse.Models;

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
        public User Login(string username, string password)
        {
            try
            {
                using var db = _dbContextFactory.CreateDbContext();
                // 注意：在实际项目中，密码应该加密存储和验证
                return db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"用户登录时发生错误: {ex.Message}");
                return null;
            }
        }

        // TODO: 在这里添加其他用户相关的业务逻辑方法
        // 例如：GetAllUsers(), AddUser(), UpdateUser(), DeleteUser() 等
    }
} 