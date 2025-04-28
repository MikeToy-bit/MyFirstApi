using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Models
{
    // 定义一个T_SYS_User类
    public class T_SYS_RoleMenusModel
    {
        // 角色菜单编号
        [Key]
        public required string RoleMenuId { get; set; }
        // 角色编号
        public required string RoleId { get; set; }
        // 菜单编号
        public required string MenuId { get; set; }
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