using System.Collections.Generic;
using System.Threading.Tasks;
using InkHouse.Models;

namespace InkHouse.Services.Interfaces
{
    /// <summary>
    /// 为普通用户提供图书相关服务的接口
    /// </summary>
    public interface IUserBookService
    {
        /// <summary>
        /// 获取所有图书列表
        /// </summary>
        Task<IEnumerable<Book>> GetAllBooksAsync();

        /// <summary>
        /// 根据关键字搜索图书
        /// </summary>
        Task<IEnumerable<Book>> SearchBooksAsync(string keyword);

        /// <summary>
        /// 用户借阅图书
        /// </summary>
        Task BorrowBookAsync(int bookId, int userId);
    }
} 