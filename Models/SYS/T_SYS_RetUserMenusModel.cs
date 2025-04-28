using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Models
{
    // 定义一个T_SYS_User类
    public class T_SYS_RetUserMenusModel
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
    }

     public class T_SYS_MenusChildrenModel
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
    }
}    