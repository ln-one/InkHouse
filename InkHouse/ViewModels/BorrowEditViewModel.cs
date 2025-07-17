using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InkHouse.Models;
using InkHouse.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace InkHouse.ViewModels
{
    /// <summary>
    /// 借阅编辑视图模型
    /// 用于处理借书和还书操作
    /// </summary>
    public partial class BorrowEditViewModel : ViewModelBase
    {
        private readonly BorrowRecordService _borrowRecordService;
        private readonly BookService _bookService;
        private readonly UserService _userService;

        [ObservableProperty]
        private bool _isBorrowMode = true; // true为借书模式，false为还书模式

        [ObservableProperty]
        private Book? _selectedBook;

        [ObservableProperty]
        private User? _selectedUser;

        [ObservableProperty]
        private BorrowRecord? _selectedBorrowRecord;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        // 可借图书列表
        public ObservableCollection<Book> AvailableBooks { get; set; } = new();

        // 用户列表
        public ObservableCollection<User> Users { get; set; } = new();

        // 借阅记录列表（还书模式）
        public ObservableCollection<BorrowRecord> BorrowRecords { get; set; } = new();

        public BorrowEditViewModel(BorrowRecordService borrowRecordService, BookService bookService, UserService userService)
        {
            _borrowRecordService = borrowRecordService;
            _bookService = bookService;
            _userService = userService;
            
            // 初始化时加载数据
            _ = LoadAvailableBooksAsync();
            _ = LoadUsersAsync();
        }

        /// <summary>
        /// 加载可借图书
        /// </summary>
        [RelayCommand]
        public async Task LoadAvailableBooksAsync()
        {
            try
            {
                IsLoading = true;
                var books = await _bookService.GetAllBooksAsync();
                var availableBooks = books.Where(b => b.AvailableCount > 0).ToList();

                AvailableBooks.Clear();
                foreach (var book in availableBooks)
                {
                    AvailableBooks.Add(book);
                }
            }
            catch (Exception ex)
            {
                // TODO: 显示错误消息
                Console.WriteLine($"加载可借图书失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 加载用户列表
        /// </summary>
        [RelayCommand]
        public async Task LoadUsersAsync()
        {
            try
            {
                IsLoading = true;
                var users = await _userService.GetAllUsersAsync();

                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载用户列表失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 加载借阅记录（还书模式）
        /// </summary>
        [RelayCommand]
        public async Task LoadBorrowRecordsAsync()
        {
            try
            {
                IsLoading = true;
                var records = await _borrowRecordService.GetUnreturnedBorrowRecordsAsync();

                BorrowRecords.Clear();
                foreach (var record in records)
                {
                    BorrowRecords.Add(record);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载借阅记录失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 搜索可借图书
        /// </summary>
        [RelayCommand]
        public async Task SearchAvailableBooksAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadAvailableBooksAsync();
                return;
            }

            try
            {
                IsLoading = true;
                var books = await _bookService.SearchBooksAsync(SearchText);
                var availableBooks = books.Where(b => b.AvailableCount > 0).ToList();

                AvailableBooks.Clear();
                foreach (var book in availableBooks)
                {
                    AvailableBooks.Add(book);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索可借图书失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        [RelayCommand]
        public async Task SearchUsersAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadUsersAsync();
                return;
            }

            try
            {
                IsLoading = true;
                var users = await _userService.SearchUsersAsync(SearchText);

                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索用户失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 搜索借阅记录
        /// </summary>
        [RelayCommand]
        public async Task SearchBorrowRecordsAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadBorrowRecordsAsync();
                return;
            }

            try
            {
                IsLoading = true;
                var records = await _borrowRecordService.SearchBorrowRecordsAsync(SearchText);
                var unreturnedRecords = records.Where(r => r.ReturnDate == null).ToList();

                BorrowRecords.Clear();
                foreach (var record in unreturnedRecords)
                {
                    BorrowRecords.Add(record);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索借阅记录失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 借书操作
        /// </summary>
        [RelayCommand]
        public async Task BorrowBookAsync()
        {
            if (SelectedBook == null)
            {
                await ShowMessageAsync("请选择要借阅的图书！");
                return;
            }

            if (SelectedUser == null)
            {
                await ShowMessageAsync("请选择借阅用户！");
                return;
            }

            try
            {
                IsLoading = true;
                await _borrowRecordService.BorrowBookAsync(SelectedBook.Id, SelectedUser.Id);
                
                // 重新加载可借图书列表
                await LoadAvailableBooksAsync();
                
                // 清空选择
                SelectedBook = null;
                SelectedUser = null;
                
                await ShowMessageAsync("借书成功！");
            }
            catch (Exception ex)
            {
                await ShowMessageAsync($"借书失败：{ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 还书操作
        /// </summary>
        [RelayCommand]
        public async Task ReturnBookAsync()
        {
            if (SelectedBorrowRecord == null)
            {
                await ShowMessageAsync("请选择要归还的借阅记录！");
                return;
            }

            try
            {
                IsLoading = true;
                await _borrowRecordService.ReturnBookAsync(SelectedBorrowRecord.Id);
                
                // 重新加载借阅记录列表
                await LoadBorrowRecordsAsync();
                
                // 清空选择
                SelectedBorrowRecord = null;
                
                await ShowMessageAsync("还书成功！");
            }
            catch (Exception ex)
            {
                await ShowMessageAsync($"还书失败：{ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 切换模式
        /// </summary>
        [RelayCommand]
        public async Task SwitchModeAsync()
        {
            IsBorrowMode = !IsBorrowMode;
            
            if (IsBorrowMode)
            {
                await LoadAvailableBooksAsync();
                await LoadUsersAsync();
            }
            else
            {
                await LoadBorrowRecordsAsync();
            }
        }

        // 简单的消息弹窗
        private async Task ShowMessageAsync(string message)
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
                {
                    Window? dialog = null;
                    dialog = new Window
                    {
                        Title = "提示",
                        Width = 300,
                        Height = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Content = new StackPanel
                        {
                            Children =
                            {
                                new TextBlock { Text = message, Margin = new Thickness(20), TextWrapping = TextWrapping.Wrap },
                                new Button { Content = "确定", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, Margin = new Thickness(0,10,0,0), Command = new RelayCommand(() => dialog?.Close()) }
                            }
                        }
                    };
                    await dialog.ShowDialog(desktop.MainWindow);
                }
            });
        }
    }
} 