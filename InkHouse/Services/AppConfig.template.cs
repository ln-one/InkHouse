using System;

namespace InkHouse.Services
{
    /// <summary>
    /// 应用程序配置管理类
    /// 统一管理数据库连接字符串等配置信息
    /// 
    /// 使用说明：
    /// 1. 将此文件重命名为 AppConfig.cs
    /// 2. 修改下面的数据库连接信息为你的实际配置
    /// 3. 确保 AppConfig.cs 已添加到 .gitignore 中（防止密码泄露）
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// 数据库连接字符串
        /// 请根据你的实际数据库信息修改这些参数
        /// </summary>
        public static string DatabaseConnectionString { get; set; } = 
            "server=47.93.254.172;port=3306;database=InternShip;user=temp_two;password=PasswordForTemp2!;";

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public static string AppName { get; set; } = "InkHouse 图书管理系统";

        /// <summary>
        /// 应用程序版本
        /// </summary>
        public static string AppVersion { get; set; } = "1.0.0";

        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        public static bool IsDebugMode { get; set; } = true;

        /// <summary>
        /// 设置数据库连接字符串
        /// </summary>
        /// <param name="server">数据库服务器地址</param>
        /// <param name="port">端口号</param>
        /// <param name="database">数据库名</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public static void SetDatabaseConnection(string server, int port, string database, string username, string password)
        {
            DatabaseConnectionString = $"server={server};port={port};database={database};user={username};password={password};";
        }
    }
} 