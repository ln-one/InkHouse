using InkHouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InkHouse.Services
{
    // 数据库上下文工厂类，用于创建数据库上下文实例
    public class DbContextFactory
    {
        // 数据库连接字符串
        private readonly string _connectionString;

        // 构造函数，传入数据库连接字符串
        public DbContextFactory()
        {
            // 从配置文件或环境变量获取连接字符串
            _connectionString = AppConfig.DatabaseConnectionString;
        }

        // 创建数据库上下文实例的方法
        public InkHouseContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<InkHouseContext>();
            // 使用MySQL数据库，自动检测版本
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            return new InkHouseContext(optionsBuilder.Options);
            
            
        }
    }
} 