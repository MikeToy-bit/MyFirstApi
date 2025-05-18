using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyFirstApi.Models;

namespace MyFirstApi.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                
                // 捕获非异常性HTTP错误状态码
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleUnauthorizedAsync(context);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await HandleForbiddenAsync(context);
                }
                else if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandleNotFoundAsync(context);
                }
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("业务异常: {Message}", ex.Message);
                await HandleBusinessExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "未处理的异常: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleBusinessExceptionAsync(HttpContext context, BusinessException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;

            ApiResponse<object> response;
            
            switch (exception.StatusCode)
            {
                case StatusCodes.Status404NotFound:
                    response = ApiResponse<object>.NotFound(exception.Message);
                    break;
                case StatusCodes.Status403Forbidden:
                    response = ApiResponse<object>.Forbidden(exception.Message);
                    break;
                case StatusCodes.Status401Unauthorized:
                    response = ApiResponse<object>.Unauthorized(exception.Message);
                    break;
                default:
                    response = new ApiResponse<object>(exception.StatusCode, exception.Message, false);
                    break;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleUnauthorizedAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var response = ApiResponse<object>.Unauthorized("用户未授权或Token已过期，请重新登录");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleForbiddenAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            var response = ApiResponse<object>.Forbidden("您没有权限访问此资源");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleNotFoundAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var response = ApiResponse<object>.NotFound("请求的资源不存在");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse<object>.Error($"内部服务器错误: {exception.Message}");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
} 