using System;
using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models
{
    // 用户查询参数DTO
    public class UserQueryDTO
    {
        public string? EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? OrgCode { get; set; }
        public string? OrgName { get; set; }
        public string? PostCode { get; set; }
        public string? PostName { get; set; }
        public bool? Sex { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IdCard { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // 用户创建请求DTO
    public class UserCreateRequestDTO
    {
        [Required(ErrorMessage = "员工编号不能为空")]
        public string EmpCode { get; set; }
        
        public string? EmpName { get; set; }
        
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        
        public string? OrgCode { get; set; }
        public string? OrgName { get; set; }
        public string? PostCode { get; set; }
        public string? PostName { get; set; }
        public bool? Sex { get; set; }
        public string? IdType { get; set; }
        public string? IdCard { get; set; }
        public DateTime? Birthday { get; set; }
        public string? PhoneNumber { get; set; }
    }

    // 用户更新请求DTO
    public class UserUpdateRequestDTO
    {
        [Required(ErrorMessage = "员工编号不能为空")]
        public string EmpCode { get; set; }
        
        public string? EmpName { get; set; }
        public string? Password { get; set; }
        public string? OrgCode { get; set; }
        public string? OrgName { get; set; }
        public string? PostCode { get; set; }
        public string? PostName { get; set; }
        public bool? Sex { get; set; }
        public string? IdType { get; set; }
        public string? IdCard { get; set; }
        public DateTime? Birthday { get; set; }
        public string? PhoneNumber { get; set; }
    }

    // 用户响应DTO（用于返回，不包含密码等敏感信息）
    public class UserResponseDTO
    {
        public string EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? OrgCode { get; set; }
        public string? OrgName { get; set; }
        public string? PostCode { get; set; }
        public string? PostName { get; set; }
        public bool? Sex { get; set; }
        public string? IdType { get; set; }
        public string? IdCard { get; set; }
        public DateTime? Birthday { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CreateEmpCode { get; set; }
        public string? CreateEmpName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? ModifyEmpCode { get; set; }
        public string? ModifyEmpName { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
} 