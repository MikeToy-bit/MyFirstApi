using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public class T_SYS_userInfoService
    {
        private readonly TestDbContext _context;

        public T_SYS_userInfoService(TestDbContext context)
        {
            _context = context;
        }
        // 获取用户信息
        public async Task<List<T_SYS_UserModel>> GetUserInfo()
        {
            return await _context.T_SYS_User.ToListAsync();
        }
        // 添加用户信息
        public async Task<T_SYS_UserModel> AddUserInfo(T_SYS_UserModel userInfo)
        {
            _context.T_SYS_User.Add(userInfo);
            await _context.SaveChangesAsync();
            return userInfo;
        }
        // 更新用户信息
        public async Task<T_SYS_UserModel> UpdateUserInfo(T_SYS_UserModel userInfo)
        {
            _context.T_SYS_User.Update(userInfo);
            await _context.SaveChangesAsync();
            return userInfo;
        }
        // 删除用户信息
        public async Task<bool> DeleteUserInfo(string empCode)
        {
            var userInfo = await _context.T_SYS_User.FindAsync(empCode);
            if (userInfo == null)
            {
                return false;   
            }
            _context.T_SYS_User.Remove(userInfo);
            await _context.SaveChangesAsync();
            return true;
        }
        // 根据员工编号获取用户信息
        public async Task<T_SYS_UserModel?> GetUserInfoByEmpCode(string empCode)
        {
            return await _context.T_SYS_User.FindAsync(empCode);
        }   
        // 根据员工编号获取用户菜单
        public async Task<List<T_SYS_RetUserMenusModel>> GetMenusByEmpCode(string empCode)
        {
            try
            {
                // 1. 获取所有菜单（扁平结构）
                var allMenus = await _context.T_SYS_UserRoles
                            .Where(ur => ur.EmpCode == empCode && 
                                        ur.User.IsDeleted == false && 
                                        ur.Role != null && 
                                        ur.Role.IsDeleted == false)
                            .SelectMany(ur => ur.Role.RoleMenus)  // 直接展开 RoleMenus
                            .Where(rm => rm.Menu != null && 
                                        rm.Menu.IsDeleted == false)
                    .Select(rm => new T_SYS_RetUserMenusModel
                    {
                        MenuId = rm.Menu.MenuId,
                        MenuName = rm.Menu.MenuName,
                        MenuType = rm.Menu.MenuType,
                        MenuUrl = rm.Menu.MenuUrl,
                        MenuIcon = rm.Menu.MenuIcon,
                        ParentMenuId = rm.Menu.ParentMenuId,
                        MenuIdentifier = rm.Menu.MenuIdentifier,
                        MenuSort = rm.Menu.MenuSort
                    })
                    .Distinct()
                    .ToListAsync();

                // 2. 构建层级结构
                var rootMenus = new List<T_SYS_RetUserMenusModel>();
                var menuDict = allMenus.OrderBy(m => m.MenuSort).ToDictionary(m => m.MenuId);

                foreach (var menu in allMenus.OrderBy(m => m.MenuSort))
                {
                    // 如果是根菜单
                    if (string.IsNullOrEmpty(menu.ParentMenuId))
                    {
                        rootMenus.Add(menu);
                    }
                    // 如果有父菜单，添加到父菜单的子菜单列表中
                    else if (menuDict.ContainsKey(menu.ParentMenuId))
                    {
                        menuDict[menu.ParentMenuId].Children.Add(menu);
                    }
                }

                return rootMenus; // 返回根菜单列表（包含子菜单）
            }
            catch (Exception ex)
            {
                throw new Exception($"获取用户菜单失败: {ex.Message}");
            }
        }
    }
}    