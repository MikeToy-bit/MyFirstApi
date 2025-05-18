using System;

namespace MyFirstApi.Models
{
    /// <summary>
    /// 业务逻辑异常类，用于处理业务规则违反等情况
    /// </summary>
    public class BusinessException : Exception
    {
        public int StatusCode { get; }

        public BusinessException(string message) : base(message)
        {
            StatusCode = 400; // 默认为400 Bad Request
        }

        public BusinessException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// 创建一个表示资源未找到的异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>BusinessException</returns>
        public static BusinessException NotFound(string message = "请求的资源不存在")
        {
            return new BusinessException(message, 404);
        }

        /// <summary>
        /// 创建一个表示无权限的异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>BusinessException</returns>
        public static BusinessException Forbidden(string message = "无权限访问此资源")
        {
            return new BusinessException(message, 403);
        }

        /// <summary>
        /// 创建一个表示未授权的异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>BusinessException</returns>
        public static BusinessException Unauthorized(string message = "用户未授权")
        {
            return new BusinessException(message, 401);
        }
    }
} 