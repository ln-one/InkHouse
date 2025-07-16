using Microsoft.EntityFrameworkCore;

namespace InkHouse.Models
{
    // 数据库上下文类，管理所有实体与数据库的交互
    public class InkHouseContext : DbContext
    {
        // 用户表
        public DbSet<User> Users { get; set; }
        // 图书表
        public DbSet<Book> Books { get; set; }
        // 借阅记录表
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        // 座位表
        public DbSet<Seat> Seats { get; set; }
        // 座位预约记录表
        public DbSet<SeatReservation> SeatReservations { get; set; }

        // 构造函数，传入数据库配置参数
        public InkHouseContext(DbContextOptions<InkHouseContext> options) : base(options)
        {
        }

        // 配置模型（可用于自定义表结构等）
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
} 