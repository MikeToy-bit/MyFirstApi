using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Models
{
    // 定义一个T_SYS_User类
    // 定义一个T_SYS_User类
    public class T_SYS_UserLoginModel
    {
        // 员工编号
        public required string EmpCode { get; set; }
        // 密码
        public required string Password { get; set; }

    }
}    