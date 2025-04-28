using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Models
{
    // 定义一个T_SYS_User类
    public class T_SYS_MenusModel
    {
        // 菜单编号
        [Key]
        public required string MenuId { get; set; }
        // 菜单名称
        public string? MenuName { get; set; }
        // 菜单类型
        public required string MenuType { get; set; }
        // 菜单标识
        public string? MenuIdentifier { get; set; }
        // 菜单图标
        public string? MenuIcon { get; set; }
        // 菜单URL
        public string? MenuUrl { get; set; }
        // 父菜单编号
        public string? ParentMenuId { get; set; }
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