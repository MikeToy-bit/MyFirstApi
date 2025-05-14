using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MyFirstApi.Models
{
    // 定义一个T_SYS_User类
    public class T_SYS_UserModel
    {
        // 员工编号
        [Key]
        public required string EmpCode { get; set; }
        // 员工姓名
        public string? EmpName { get; set; }
        // 密码
        public required string Password { get; set; }
        // 组织编号
        public string? OrgCode { get; set; }
        // 组织名称
        public string? OrgName { get; set; }
        // 岗位编号
        public string? PostCode { get; set; }
        // 岗位名称
        public string? PostName { get; set; }
        // 性别
        public bool? Sex { get; set; }
        // 身份证类型
        public string? IdType { get; set; }
        // 身份证号码
        public string? IdCard { get; set; }
        // 出生日期
        public DateTime? Birthday { get; set; }
        // 电话号码
        public string? PhoneNumber { get; set; }
        // 创建人编号
        public string? CreateEmpCode { get; set; }
        // 创建人姓名
        public string? CreateEmpName { get; set; }
        // 创建时间
        public DateTime? CreateTime { get; set; }
        // 修改人编号
        public string? ModifyEmpCode { get; set; }
        // 修改人姓名
        public string? ModifyEmpName { get; set; }
        // 修改时间
        public DateTime? ModifyTime { get; set; }
        // 是否删除
        public bool? IsDeleted { get; set; }

        // 导航属性
        public virtual ICollection<T_SYS_UserRolesModel> UserRoles { get; set; } = new List<T_SYS_UserRolesModel>();
    }
}    