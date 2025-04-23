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
    }
} 