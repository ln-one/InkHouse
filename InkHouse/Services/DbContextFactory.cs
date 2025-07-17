using InkHouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace InkHouse.Services
{
    // 数据库上下文工厂类，用于创建数据库上下文实例
    public class DbContextFactory : IDesignTimeDbContextFactory<InkHouseContext>
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

        // 实现IDesignTimeDbContextFactory接口的方法，用于EF Core迁移工具
        public InkHouseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InkHouseContext>();
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            return new InkHouseContext(optionsBuilder.Options);
        }


        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <returns>连接是否成功</returns>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                Console.WriteLine($"正在测试数据库连接...");
                Console.WriteLine($"连接字符串: {_connectionString}");
                
                using var context = CreateDbContext();
                await context.Database.OpenConnectionAsync();
                Console.WriteLine("数据库连接成功！");
                
                // 测试查询各个表的数据量
                var userCount = await context.Users.CountAsync();
                var bookCount = await context.Books.CountAsync();
                var borrowCount = await context.BorrowRecords.CountAsync();
                var seatCount = await context.Seats.CountAsync();
                var reservationCount = await context.SeatReservations.CountAsync();
                
                Console.WriteLine($"数据库表数据统计:");
                Console.WriteLine($"  Users: {userCount} 条");
                Console.WriteLine($"  Books: {bookCount} 条");
                Console.WriteLine($"  BorrowRecords: {borrowCount} 条");
                Console.WriteLine($"  Seats: {seatCount} 条");
                Console.WriteLine($"  SeatReservations: {reservationCount} 条");
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"数据库连接测试失败: {ex.Message}");
                Console.WriteLine($"异常堆栈: {ex.StackTrace}");
                return false;
            }

        }
    }
} 