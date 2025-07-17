using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;
using ReactiveUI;

namespace InkHouse.ViewModels
{
    /// <summary>
    /// ViewModel基类
    /// 提供通用的属性和方法，简化ViewModel开发
    /// </summary>
    public class ViewModelBase : ObservableObject
    {
        private bool _isLoading;
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;
        private bool _disposed = false;

        /// <summary>
        /// 是否正在加载
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// 成功消息
        /// </summary>
        public string SuccessMessage
        {
            get => _successMessage;
            set => SetProperty(ref _successMessage, value);
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="message">错误消息内容</param>
        protected void ShowError(string message)
        {
            ErrorMessage = message;
            // 3秒后自动清除错误消息
            Task.Delay(3000).ContinueWith(_ => ErrorMessage = string.Empty);
        }

        /// <summary>
        /// 显示成功消息
        /// </summary>
        /// <param name="message">成功消息内容</param>
        protected void ShowSuccess(string message)
        {
            SuccessMessage = message;
            // 3秒后自动清除成功消息
            Task.Delay(3000).ContinueWith(_ => SuccessMessage = string.Empty);
        }

        /// <summary>
        /// 清除所有消息
        /// </summary>
        protected void ClearMessages()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        /// <summary>
        /// 执行异步操作的安全方法
        /// </summary>
        /// <param name="action">要执行的操作</param>
        protected async Task ExecuteAsync(Func<Task> action)
        {
            try
            {
                IsLoading = true;
                ClearMessages();
                await action();
            }
            catch (Exception ex)
            {
                ShowError($"操作失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 执行异步操作并返回结果的安全方法
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="action">要执行的操作</param>
        /// <returns>操作结果</returns>
        protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                IsLoading = true;
                ClearMessages();
                return await action();
            }
            catch (Exception ex)
            {
                ShowError($"操作失败: {ex.Message}");
                return default(T);
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// 释放资源的受保护方法
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // 释放托管资源
                ClearMessages();
                _disposed = true;
            }
        }
        
        /// <summary>
        /// 析构函数
        /// </summary>
        ~ViewModelBase()
        {
            Dispose(false);
        }
    }
}