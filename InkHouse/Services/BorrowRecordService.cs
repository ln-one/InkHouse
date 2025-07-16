using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InkHouse.Models;

namespace InkHouse.Services
{
    /// <summary>
    /// 借阅记录服务类
    /// 负责借阅记录相关的业务逻辑操作
    /// </summary>
    public class BorrowRecordService
    {
        private readonly DbContextFactory _dbContextFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextFactory">数据库上下文工厂</param>
        public BorrowRecordService(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// 获取所有借阅记录
        /// </summary>
        /// <returns>借阅记录列表</returns>
        public async Task<List<BorrowRecord>> GetAllBorrowRecordsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
        }

        /// <summary>
        /// 根据ID获取借阅记录
        /// </summary>
        /// <param name="id">借阅记录ID</param>
        /// <returns>借阅记录对象</returns>
        public async Task<BorrowRecord?> GetBorrowRecordByIdAsync(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        /// <summary>
        /// 获取用户的借阅记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>用户的借阅记录列表</returns>
        public async Task<List<BorrowRecord>> GetBorrowRecordsByUserIdAsync(int userId, int page = 1, int pageSize = 20)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.BorrowRecords
                .AsNoTracking() // Improves query performance
                .Include(br => br.Book)
                .Where(br => br.UserId == userId)
                .OrderByDescending(br => br.BorrowDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 获取图书的借阅记录
        /// </summary>
        /// <param name="bookId">图书ID</param>
        /// <returns>图书的借阅记录列表</returns>
        public async Task<List<BorrowRecord>> GetBorrowRecordsByBookIdAsync(int bookId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.BookId == bookId)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
        }

        /// <summary>
        /// 搜索借阅记录
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <returns>匹配的借阅记录列表</returns>
        public async Task<List<BorrowRecord>> SearchBorrowRecordsAsync(string keyword)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.Book.Title.Contains(keyword) || 
                           br.Book.Author.Contains(keyword) ||
                           br.Book.ISBN.Contains(keyword) ||
                           br.User.UserName.Contains(keyword) ||
                           br.Status.Contains(keyword))
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
        }

        /// <summary>
        /// 获取未归还的借阅记录
        /// </summary>
        /// <returns>未归还的借阅记录列表</returns>
        public async Task<List<BorrowRecord>> GetUnreturnedBorrowRecordsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.ReturnDate == null)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
        }

        /// <summary>
        /// 借书操作
        /// </summary>
        /// <param name="bookId">图书ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>借阅记录</returns>
        public async Task<BorrowRecord> BorrowBookAsync(int bookId, int userId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            // 检查图书是否存在
            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new InvalidOperationException("图书不存在");
            }

            // 检查用户是否存在
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("用户不存在");
            }

            // 检查图书是否可借
            if (book.AvailableCount <= 0)
            {
                throw new InvalidOperationException("图书库存不足，无法借阅");
            }

            // 检查用户是否已借阅该图书且未归还
            var existingBorrow = await context.BorrowRecords
                .FirstOrDefaultAsync(br => br.BookId == bookId && 
                                         br.UserId == userId && 
                                         br.ReturnDate == null);
            
            if (existingBorrow != null)
            {
                throw new InvalidOperationException("您已借阅该图书且未归还");
            }

            // 创建借阅记录
            var borrowRecord = new BorrowRecord
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateTime.Now,
                ReturnDate = null,
                Status = "借出",
                IsReturned = false
            };

            // 更新图书库存
            book.AvailableCount--;
            if (book.AvailableCount == 0)
            {
                book.IsAvailable = false;
            }

            context.BorrowRecords.Add(borrowRecord);
            await context.SaveChangesAsync();
            
            return borrowRecord;
        }

        /// <summary>
        /// 还书操作
        /// </summary>
        /// <param name="borrowRecordId">借阅记录ID</param>
        /// <returns>是否还书成功</returns>
        public async Task<bool> ReturnBookAsync(int borrowRecordId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            var borrowRecord = await context.BorrowRecords
                .Include(br => br.Book)
                .FirstOrDefaultAsync(br => br.Id == borrowRecordId);
            
            if (borrowRecord == null)
            {
                return false;
            }

            if (borrowRecord.ReturnDate != null)
            {
                throw new InvalidOperationException("该图书已经归还");
            }

            // 更新借阅记录
            borrowRecord.ReturnDate = DateTime.Now;
            borrowRecord.Status = "归还";
            borrowRecord.IsReturned = true;

            // 更新图书库存
            borrowRecord.Book.AvailableCount++;
            if (borrowRecord.Book.AvailableCount > 0)
            {
                borrowRecord.Book.IsAvailable = true;
            }

            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 获取借阅统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public async Task<(int totalBorrows, int activeBorrows, int returnedBorrows)> GetBorrowStatisticsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            var totalBorrows = await context.BorrowRecords.CountAsync();
            var activeBorrows = await context.BorrowRecords.CountAsync(br => br.ReturnDate == null);
            var returnedBorrows = totalBorrows - activeBorrows;

            return (totalBorrows, activeBorrows, returnedBorrows);
        }

        /// <summary>
        /// 删除借阅记录
        /// </summary>
        /// <param name="id">借阅记录ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteBorrowRecordAsync(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            var borrowRecord = await context.BorrowRecords.FindAsync(id);
            if (borrowRecord == null)
            {
                return false;
            }

            // 如果记录未归还，不允许删除
            if (borrowRecord.ReturnDate == null)
            {
                throw new InvalidOperationException("无法删除未归还的借阅记录");
            }

            context.BorrowRecords.Remove(borrowRecord);
            await context.SaveChangesAsync();
            return true;
        }
    }
} 