using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InkHouse.Models;

namespace InkHouse.Services
{
    /// <summary>
    /// 图书服务类
    /// 负责图书相关的业务逻辑操作
    /// 团队成员可以在这里添加自己的业务逻辑
    /// </summary>
    public class BookService
    {
        private readonly DbContextFactory _dbContextFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextFactory">数据库上下文工厂</param>
        public BookService(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// 获取所有图书
        /// </summary>
        /// <returns>图书列表</returns>
        public async Task<List<Book>> GetAllBooksAsync(int page = 1, int pageSize = 50)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Books
                .AsNoTracking() // Improves query performance since we're only reading
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 根据ID获取图书
        /// </summary>
        /// <param name="id">图书ID</param>
        /// <returns>图书对象</returns>
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Books.FindAsync(id);
        }

        /// <summary>
        /// 搜索图书
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>匹配的图书列表</returns>
        public async Task<List<Book>> SearchBooksAsync(string keyword, int page = 1, int pageSize = 50)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Books
                .AsNoTracking() // Improves query performance
                .Where(b => EF.Functions.Like(b.Title, $"%{keyword}%") || 
                           EF.Functions.Like(b.Author, $"%{keyword}%") || 
                           EF.Functions.Like(b.ISBN, $"%{keyword}%") ||
                           EF.Functions.Like(b.Publisher, $"%{keyword}%"))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 添加图书
        /// </summary>
        /// <param name="book">图书对象</param>
        /// <returns>添加的图书</returns>
        public async Task<Book> AddBookAsync(Book book)
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            // 检查ISBN是否已存在
            if (await context.Books.AnyAsync(b => b.ISBN == book.ISBN))
            {
                throw new InvalidOperationException("ISBN已存在，无法添加重复的图书");
            }

            context.Books.Add(book);
            await context.SaveChangesAsync();
            return book;
        }

        /// <summary>
        /// 更新图书
        /// </summary>
        /// <param name="book">图书对象</param>
        /// <returns>更新后的图书</returns>
        public async Task<Book> UpdateBookAsync(Book book)
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            var existingBook = await context.Books.FindAsync(book.Id);
            if (existingBook == null)
            {
                throw new InvalidOperationException("图书不存在");
            }

            // 检查ISBN是否被其他图书使用
            if (await context.Books.AnyAsync(b => b.ISBN == book.ISBN && b.Id != book.Id))
            {
                throw new InvalidOperationException("ISBN已被其他图书使用");
            }

            // 更新属性
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.ISBN = book.ISBN;
            existingBook.Publisher = book.Publisher;
            existingBook.Year = book.Year;
            existingBook.TotalCount = book.TotalCount;
            existingBook.AvailableCount = book.AvailableCount;
            existingBook.IsAvailable = book.IsAvailable;

            await context.SaveChangesAsync();
            return existingBook;
        }

        /// <summary>
        /// 删除图书
        /// </summary>
        /// <param name="id">图书ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteBookAsync(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            // 检查是否有未归还的借阅记录
            var hasActiveBorrows = await context.BorrowRecords
                .AnyAsync(br => br.BookId == id && br.ReturnDate == null);
            
            if (hasActiveBorrows)
            {
                throw new InvalidOperationException("该图书有未归还的借阅记录，无法删除");
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 获取图书统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public async Task<(int totalBooks, int availableBooks, int borrowedBooks)> GetBookStatisticsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            var totalBooks = await context.Books.SumAsync(b => b.TotalCount);
            var availableBooks = await context.Books.SumAsync(b => b.AvailableCount);
            var borrowedBooks = totalBooks - availableBooks;

            return (totalBooks, availableBooks, borrowedBooks);
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
            
            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new InvalidOperationException("图书不存在");
            }

            if (book.AvailableCount <= 0)
            {
                throw new InvalidOperationException("图书库存不足");
            }

            // 创建借阅记录
            var borrowRecord = new BorrowRecord
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateTime.Now,
                // DueDate = DateTime.Now.AddDays(30), // 默认借阅30天（如有）
                ReturnDate = null,
                Status = "借出"
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

            // 更新图书库存
            if (borrowRecord.Book != null)
            {
                borrowRecord.Book.AvailableCount++;
                if (borrowRecord.Book.AvailableCount > 0)
                {
                    borrowRecord.Book.IsAvailable = true;
                }
            }

            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 按类型分页查询图书
        /// </summary>
        public async Task<List<Book>> GetBooksByTypeAsync(string? type, int page = 1, int pageSize = 50)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var query = context.Books.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(type) && type != "全部")
            {
                query = query.Where(b => b.Type == type);
            }
            return await query.OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 获取所有已存在的图书类型
        /// </summary>
        public async Task<List<string>> GetAllBookTypesAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Books
                .Where(b => b.Type != null && b.Type != "")
                .Select(b => b.Type!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }
    }
}