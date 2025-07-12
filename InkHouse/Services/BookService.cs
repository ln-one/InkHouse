using System;
using System.Collections.Generic;
using System.Linq;
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

        // TODO: 在这里添加图书相关的业务逻辑方法
        // 例如：GetAllBooks(), AddBook(), UpdateBook(), DeleteBook() 等
    }
} 