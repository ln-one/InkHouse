using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InkHouse.Models;
using InkHouse.Services;
using InkHouse.Views;

namespace InkHouse.ViewModels
{
    public partial class BookEditViewModel : ViewModelBase
    {
        private readonly BookService _bookService;
        private readonly Book? _originalBook;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _author = string.Empty;

        [ObservableProperty]
        private string _isbn = string.Empty;

        [ObservableProperty]
        private string _publisher = string.Empty;

        [ObservableProperty]
        private int _year = DateTime.Now.Year;

        [ObservableProperty]
        private int _totalCount = 1;

        [ObservableProperty]
        private int _availableCount = 1;

        [ObservableProperty]
        private bool _isAvailable = true;

        [ObservableProperty]
        private bool _isLoading = false;

        public BookEditViewModel(BookService bookService, Book? book = null)
        {
            _bookService = bookService;
            _originalBook = book;

            if (book != null)
            {
                // 编辑模式：填充现有数据
                Title = book.Title;
                Author = book.Author;
                this.Isbn = book.ISBN;
                Publisher = book.Publisher;
                Year = book.Year;
                TotalCount = book.TotalCount;
                AvailableCount = book.AvailableCount;
                IsAvailable = book.IsAvailable;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            try
            {
                IsLoading = true;

                // 验证必填字段
                if (string.IsNullOrWhiteSpace(Title))
                {
                    // TODO: 显示错误消息
                    return;
                }

                if (string.IsNullOrWhiteSpace(Author))
                {
                    // TODO: 显示错误消息
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.Isbn))
                {
                    // TODO: 显示错误消息
                    return;
                }

                if (string.IsNullOrWhiteSpace(Publisher))
                {
                    // TODO: 显示错误消息
                    return;
                }

                // 验证数量
                if (TotalCount < 1)
                {
                    // TODO: 显示错误消息
                    return;
                }

                if (AvailableCount < 0 || AvailableCount > TotalCount)
                {
                    // TODO: 显示错误消息
                    return;
                }

                var book = new Book
                {
                    Title = Title,
                    Author = Author,
                    ISBN = this.Isbn,
                    Publisher = Publisher,
                    Year = Year,
                    TotalCount = TotalCount,
                    AvailableCount = AvailableCount,
                    IsAvailable = IsAvailable
                };

                if (_originalBook != null)
                {
                    // 编辑模式
                    book.Id = _originalBook.Id;
                    await _bookService.UpdateBookAsync(book);
                }
                else
                {
                    // 新增模式
                    await _bookService.AddBookAsync(book);
                }

                // 关闭对话框
                if (App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
                {
                    foreach (var window in desktop.Windows)
                    {
                        if (window is BookEditDialog dialog)
                        {
                            dialog.Close();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: 显示错误消息
                Console.WriteLine($"保存图书失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            // 关闭对话框
            if (App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (var window in desktop.Windows)
                {
                    if (window is BookEditDialog dialog)
                    {
                        dialog.Close();
                        break;
                    }
                }
            }
        }
    }
} 