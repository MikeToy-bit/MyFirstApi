using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Models
{
    // 定义一个T_SYS_User类
    public class T_SYS_UserRolesModel
    {
        // 用户角色编号
        [Key]
        public required string EmpRoleId { get; set; }
        // 用户编号
        public required string EmpCode { get; set; }
        // 角色编号
        public required string RoleId { get; set; }
        // 创建人编号
        public string? CreateEmpCode { get; set; }
        // 创建人姓名
        public string? CreateEmpName { get; set; }
        // 创建时间
        public string? CreateTime { get; set; }
        // 修改人编号
        public string? ModifyEmpCode { get; set; }
        // 修改人姓名
        public string? ModifyEmpName { get; set; }
        // 修改时间
        public string? ModifyTime { get; set; }
        // 是否删除
        public bool? IsDeleted { get; set; }
    }
}    