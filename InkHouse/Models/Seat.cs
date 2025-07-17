using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkHouse.Models
{
    // 座位实体类，对应数据库中的 Seats 表
    [Table("Seats")]
    public class Seat
    {
        // 主键，自增ID
        [Key]
        public int Id { get; set; }
        // 座位编号，必填，最大长度10
        [Required]
        [MaxLength(10)]
        public required string SeatNumber { get; set; }
        // 座位状态：空闲、被预约、使用中
        [Required]
        [MaxLength(10)]
        public required string Status { get; set; } // Free/Reserved/Occupied
        // 当前预约用户ID，可为空
        public int? CurrentUserId { get; set; }
        // 导航属性：当前预约用户
        [ForeignKey("CurrentUserId")]
        public User? CurrentUser { get; set; }
    }
} 