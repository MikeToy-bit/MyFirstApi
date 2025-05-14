using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    // 角色菜单服务接口
    public interface IT_SYS_RoleMenuService
    {
        // 获取所有角色菜单
        Task<List<T_SYS_RoleMenusModel>> GetRoleMenus();
        // 根据角色获取角色菜单
        Task<List<T_SYS_RoleMenusModel>> GetRoleMenusByRole(string roleId);
        // 根据菜单获取角色菜单
        Task<List<T_SYS_RoleMenusModel>> GetRoleMenusByMenu(string menuId);
        // 添加角色菜单
        Task<T_SYS_RoleMenusModel> AddRoleMenu(T_SYS_RoleMenusModel roleMenu);
        // 删除角色菜单
        Task<bool> DeleteRoleMenu(string roleMenuId);
        // 根据角色删除角色菜单
        Task<bool> DeleteRoleMenusByRole(string roleId);
        // 根据菜单删除角色菜单
        Task<bool> DeleteRoleMenusByMenu(string menuId);
    }
    // 角色菜单服务实现
    public class T_SYS_RoleMenuService : IT_SYS_RoleMenuService
    {
        private readonly TestDbContext _context;

        public T_SYS_RoleMenuService(TestDbContext context)
        {
            _context = context;
        }
        // 获取所有角色菜单
        public async Task<List<T_SYS_RoleMenusModel>> GetRoleMenus()
        {
            return await _context.T_SYS_RoleMenus.ToListAsync();
        }
        // 根据角色获取角色菜单
        public async Task<List<T_SYS_RoleMenusModel>> GetRoleMenusByRole(string roleId)
        {
            return await _context.T_SYS_RoleMenus
                .Where(rm => rm.RoleId == roleId)
                .ToListAsync();
        }
        // 根据菜单获取角色菜单
        public async Task<List<T_SYS_RoleMenusModel>> GetRoleMenusByMenu(string menuId)
        {
            return await _context.T_SYS_RoleMenus
                .Where(rm => rm.MenuId == menuId)
                .ToListAsync();
        }
        // 添加角色菜单 
        public async Task<T_SYS_RoleMenusModel> AddRoleMenu(T_SYS_RoleMenusModel roleMenu)
        {
            _context.T_SYS_RoleMenus.Add(roleMenu);
            await _context.SaveChangesAsync();
            return roleMenu;
        }
        // 删除角色菜单
        public async Task<bool> DeleteRoleMenu(string roleMenuId)
        {
            var roleMenu = await _context.T_SYS_RoleMenus.FindAsync(roleMenuId);
            if (roleMenu == null)
            {
                return false;
            }
            _context.T_SYS_RoleMenus.Remove(roleMenu);
            await _context.SaveChangesAsync();
            return true;
        }
        // 根据角色删除角色菜单
        public async Task<bool> DeleteRoleMenusByRole(string roleId)
        {
            var roleMenus = await _context.T_SYS_RoleMenus
                .Where(rm => rm.RoleId == roleId)
                .ToListAsync();
            _context.T_SYS_RoleMenus.RemoveRange(roleMenus);
            await _context.SaveChangesAsync();
            return true;
        }
        // 根据菜单删除角色菜单 
        public async Task<bool> DeleteRoleMenusByMenu(string menuId)
        {
            var roleMenus = await _context.T_SYS_RoleMenus
                .Where(rm => rm.MenuId == menuId)
                .ToListAsync();
            _context.T_SYS_RoleMenus.RemoveRange(roleMenus);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 