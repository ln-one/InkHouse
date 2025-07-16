using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InkHouse.Models;
using BC = BCrypt.Net.BCrypt;

namespace InkHouse.Services
{
    /// <summary>
    /// 注册结果类
    /// </summary>
    public class RegisterResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public User? User { get; set; }
    }

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

                // 先根据用户名查找用户
                var user = db.Users.FirstOrDefault(u => u.UserName == username);
                if (user == null)
                {
                    Console.WriteLine("用户不存在");
                    return null;
                }

                // 验证密码
                bool isPasswordValid;
                if (user.Password.StartsWith("$2"))
                {
                    // 新的加密密码
                    isPasswordValid = BC.Verify(password, user.Password);
                }
                else
                {
                    // 兼容旧的明文密码（用于现有数据）
                    isPasswordValid = user.Password == password;

                    // 如果验证成功，将明文密码升级为加密密码
                    if (isPasswordValid)
                    {
                        user.Password = BC.HashPassword(password);
                        db.SaveChanges();
                        Console.WriteLine("密码已升级为加密存储");
                    }
                }

                if (isPasswordValid)
                {
                    Console.WriteLine("登录验证成功");
                    return user;
                }
                else
                {
                    Console.WriteLine("密码错误");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"用户登录时发生错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns>注册结果</returns>
        public async Task<RegisterResult> RegisterAsync(User user)
        {
            try
            {
                using var db = _dbContextFactory.CreateDbContext();

                // 检查用户名是否已存在
                if (await db.Users.AnyAsync(u => u.UserName == user.UserName))
                {
                    return new RegisterResult
                    {
                        Success = false,
                        ErrorMessage = "用户名已存在，请选择其他用户名"
                    };
                }

                // 加密密码
                user.Password = BC.HashPassword(user.Password);

                // 添加用户到数据库
                db.Users.Add(user);
                await db.SaveChangesAsync();

                Console.WriteLine($"用户注册成功: {user.UserName}");

                return new RegisterResult
                {
                    Success = true,
                    User = user
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"用户注册失败: {ex.Message}");
                return new RegisterResult
                {
                    Success = false,
                    ErrorMessage = "注册失败，请稍后重试"
                };
            }
        }

        /// <summary>
        /// 检查用户名是否可用
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>是否可用</returns>
        public async Task<bool> IsUsernameAvailableAsync(string username)
        {
            try
            {
                using var db = _dbContextFactory.CreateDbContext();
                return !await db.Users.AnyAsync(u => u.UserName == username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"检查用户名可用性失败: {ex.Message}");
                return false;
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