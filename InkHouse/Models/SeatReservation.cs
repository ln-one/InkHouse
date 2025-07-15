using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkHouse.Models
{
    // 座位预约记录实体类，对应数据库中的 SeatReservations 表
    [Table("SeatReservations")]
    public class SeatReservation
    {
        // 主键，自增ID
        [Key]
        public int Id { get; set; }
        // 用户ID，外键，必填
        [Required]
        public int UserId { get; set; }
        // 座位ID，外键，必填
        [Required]
        public int SeatId { get; set; }
        // 预约时间，必填
        [Required]
        public DateTime ReserveTime { get; set; }
        // 到馆时间，可为空
        public DateTime? CheckInTime { get; set; }
        // 离馆时间，可为空
        public DateTime? CheckOutTime { get; set; }
        // 状态，必填，最大长度20（如“已预约”、“使用中”、“已离馆”）
        [Required]
        [MaxLength(20)]
        public required string Status { get; set; }
        // 导航属性：座位
        [ForeignKey("SeatId")]
        public Seat? Seat { get; set; }
        // 导航属性：用户
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
} 