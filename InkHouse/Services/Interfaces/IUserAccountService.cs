using System.Threading.Tasks;
using InkHouse.Models;

namespace InkHouse.Services.Interfaces
{
    /// <summary>
    /// 为普通用户提供账户管理服务的接口
    /// </summary>
    public interface IUserAccountService
    {
        /// <summary>
        /// 获取当前用户的个人资料
        /// </summary>
        Task<User> GetMyProfileAsync(int userId);

        /// <summary>
        /// 用户修改密码
        /// </summary>
        Task ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
} 