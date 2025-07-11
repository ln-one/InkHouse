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
        public string Title { get; set; }
        // 作者，最大长度50
        [MaxLength(50)]
        public string Author { get; set; }
        // ISBN号，最大长度20
        [MaxLength(20)]
        public string ISBN { get; set; }
        // 出版社，最大长度50
        [MaxLength(50)]
        public string Publisher { get; set; }
        // 出版年份
        public int Year { get; set; }
        // 图书总数量
        public int TotalCount { get; set; }
        // 可借数量
        public int Available { get; set; }
    }
} 