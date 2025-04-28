using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Models
{
    // 角色模型
    public class T_SYS_RolesModel
    {
        // 角色编号
        [Key]
        public required string RoleId { get; set; }
        // 角色名称
        public string? RoleName { get; set; }
        // 是否默认
        public required bool IsDefault { get; set; }
        // 描述
        public string? Description { get; set; }
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