using InkHouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace InkHouse.Services
{
    // 数据库上下文工厂类，用于创建数据库上下文实例
    public class DbContextFactory
    {
        // 数据库连接字符串
        private readonly string _connectionString;
        private readonly DbContextOptions<InkHouseContext> _options;

        // 构造函数，传入数据库连接字符串
        public DbContextFactory()
        {
            // 从配置文件或环境变量获取连接字符串
            _connectionString = AppConfig.DatabaseConnectionString;
            
            var optionsBuilder = new DbContextOptionsBuilder<InkHouseContext>();
            // 使用MySQL数据库，自动检测版本
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString), 
                options => 
                {
                    options.EnableRetryOnFailure(
                        maxRetryCount: 5, 
                        maxRetryDelay: TimeSpan.FromSeconds(30), 
                        errorNumbersToAdd: null);
                    
                    // Set command timeout to prevent long-running queries
                    options.CommandTimeout(15);
                });
                
            _options = optionsBuilder.Options;
        }

        // 创建数据库上下文实例的方法
        public InkHouseContext CreateDbContext()
        {
            return new InkHouseContext(_options);
        }
    }
} 