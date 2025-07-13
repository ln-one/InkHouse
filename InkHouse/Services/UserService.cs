using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        /// 获取所有用户
        /// </summary>
        /// <returns>用户列表</returns>
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                using var db = _dbContextFactory.CreateDbContext();
                return await db.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取用户列表失败: {ex.Message}");
                return new List<User>();
            }
        }

        /// <summary>
        /// 根据搜索条件获取用户
        /// </summary>
        /// <param name="searchText">搜索文本</param>
        /// <returns>匹配的用户列表</returns>
        public async Task<List<User>> SearchUsersAsync(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return await GetAllUsersAsync();
                }

                using var db = _dbContextFactory.CreateDbContext();
                return await db.Users
                    .Where(u => u.UserName.Contains(searchText) || u.Role.Contains(searchText))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索用户失败: {ex.Message}");
                return new List<User>();
            }
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns>是否成功</returns>
        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                using var db = _dbContextFactory.CreateDbContext();
                
                // 检查用户名是否已存在
                if (await db.Users.AnyAsync(u => u.UserName == user.UserName))
                {
                    Console.WriteLine($"用户名 '{user.UserName}' 已存在");
                    return false;
                }

                // 强制去除Role字段空格
                user.Role = user.Role.Trim();
                Console.WriteLine($"即将插入用户，Role字段内容：'{user.Role}', 长度：{user.Role.Length}");
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"添加用户失败: {ex}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner: {ex.InnerException}");
                return false;
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns>是否成功</returns>
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                using var db = _dbContextFactory.CreateDbContext();
                
                var existingUser = await db.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    Console.WriteLine($"用户ID {user.Id} 不存在");
                    return false;
                }

                // 检查用户名是否被其他用户使用
                if (await db.Users.AnyAsync(u => u.UserName == user.UserName && u.Id != user.Id))
                {
                    Console.WriteLine($"用户名 '{user.UserName}' 已被其他用户使用");
                    return false;
                }

                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.Role = user.Role;

                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新用户失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否成功</returns>
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                using var db = _dbContextFactory.CreateDbContext();
                
                var user = await db.Users.FindAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"用户ID {userId} 不存在");
                    return false;
                }

                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除用户失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户对象，如果不存在返回null</returns>
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            try
            {
                using var db = _dbContextFactory.CreateDbContext();
                return await db.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取用户失败: {ex.Message}");
                return null;
            }
        }
    }
} 