using System;

namespace MyFirstApi.Models
{   // 定义一个ApiResponse类
    public class ApiResponse<T>
    {
        // 状态码
        public int StatusCode { get; set; }
        // 消息
        public string Message { get; set; }
        // 数据
        public T? Data { get; set; }
        // 是否成功
        public bool Success { get; set; }

        // 构造函数
        public ApiResponse()
        {
            StatusCode = 200;
            Success = true;
            Message = "操作成功";
        }

        // 构造函数
        public ApiResponse(T data)
        {
            StatusCode = 200;
            Success = true;
            Message = "操作成功";
            Data = data;
        }

        // 构造函数
        public ApiResponse(int statusCode, string message, bool success = false)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }

        // 构造函数     
        public ApiResponse(int statusCode, string message, T data, bool success = false)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Success = success;
        }

        // 404 未找到资源
        public static ApiResponse<T> NotFound(string message = "资源未找到")
        {
            return new ApiResponse<T>
            {
                StatusCode = 404,
                Message = message,
                Success = false,
                Data = default
            };
        }

        // 500 服务器内部错误
        public static ApiResponse<T> Error(string message = "服务器内部错误")
        {
            return new ApiResponse<T>
            {
                StatusCode = 500,
                Message = message,
                Success = false,
                Data = default
            };
        }

        // 400 请求无效
        public static ApiResponse<T> BadRequest(string message = "请求无效")
        {
            return new ApiResponse<T>
            {
                StatusCode = 400,
                Message = message,
                Success = false,
                Data = default
            };
        }

        // 403 无权限访问
        public static ApiResponse<T> Forbidden(string message = "无权限访问")
        {
            return new ApiResponse<T>
            {
                StatusCode = 403,
                Message = message,
                Success = false,
                Data = default
            };
        }

        // 401 未授权
        public static ApiResponse<T> Unauthorized(string message = "未授权")
        {
            return new ApiResponse<T>
            {
                StatusCode = 401,
                Message = message,
                Success = false,
                Data = default
            };
        }

        // 200 操作成功
        public static ApiResponse<T> Ok(T data, string message = "操作成功")
        {
            return new ApiResponse<T>
            {
                StatusCode = 200,
                Message = message,
                Success = true,
                Data = data
            };
        }
    }
} 