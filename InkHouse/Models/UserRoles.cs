using System;

namespace InkHouse.Models
{
    /// <summary>
    /// 用户角色常量定义
    /// </summary>
    public static class UserRoles
    {
        /// <summary>
        /// 管理员角色
        /// </summary>
        public const string Admin = "Admin";
        
        /// <summary>
        /// 普通用户角色
        /// </summary>
        public const string User = "User";
        
        /// <summary>
        /// 检查是否为管理员
        /// </summary>
        /// <param name="role">角色字符串</param>
        /// <returns>是否为管理员</returns>
        public static bool IsAdmin(string role)
        {
            return role.Equals(Admin, StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// 检查是否为普通用户
        /// </summary>
        /// <param name="role">角色字符串</param>
        /// <returns>是否为普通用户</returns>
        public static bool IsUser(string role)
        {
            return role.Equals(User, StringComparison.OrdinalIgnoreCase);
        }
    }
} 