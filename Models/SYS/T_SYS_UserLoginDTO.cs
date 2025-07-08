using System;
using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models
{
    // 用户查询参数DTO
    public class T_SYS_UserLoginDTO
    {
        public string? EmpCode { get; set; }
        public string? Password { get; set; }
    }
} 