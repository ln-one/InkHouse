using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkHouse.Models
{
    // 图书实体类，对应数据库中的Books表
    [Table("Books")]
    public class Book
    {
        // 主键，自增ID
        [Key]
        public int Id { get; set; }
        // 书名，必填，最大长度100
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Author { get; set; }
        [Required]
        [MaxLength(20)]
        public required string ISBN { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Publisher { get; set; }
        // 出版年份
        public int Year { get; set; }
        // 图书总数量
        public int TotalCount { get; set; }
        // 可借数量
        public int AvailableCount { get; set; }
        // 是否可借（用于简化逻辑）
        public bool IsAvailable { get; set; } = true;
        public string? CoverImagePath { get; set; }
        public string? Type { get; set; } // 图书类型，如文学、科幻等
    }
} 