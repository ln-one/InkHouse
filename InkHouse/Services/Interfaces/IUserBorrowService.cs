using System.Collections.Generic;
using System.Threading.Tasks;
using InkHouse.Models;

namespace InkHouse.Services.Interfaces
{
    /// <summary>
    /// 为普通用户提供借阅记录相关服务的接口
    /// </summary>
    public interface IUserBorrowService
    {
        /// <summary>
        /// 获取当前用户的借阅历史
        /// </summary>
        Task<IEnumerable<BorrowRecord>> GetMyBorrowHistoryAsync(int userId);

        /// <summary>
        /// 用户归还图书
        /// </summary>
        Task ReturnBookAsync(int borrowRecordId);
    }
} 