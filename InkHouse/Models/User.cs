using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkHouse.Models
{
    // 用户实体类，对应数据库中的Users表
    [Table("Users")]
    public class User
    {
        // 主键，自增ID
        [Key]
        public int Id { get; set; }
        // 用户名，必填，最大长度50
        [Required]
        [MaxLength(50)]
        public required string UserName { get; set; }
        // 密码，必填，最大长度255（建议加密存储）
        [Required]
        [MaxLength(255)]
        public required string Password { get; set; }
        // 角色，必填，最大长度20（如Admin或User）
        [Required]
        [MaxLength(20)]
        public required string Role { get; set; } // Admin/User
    }
} 