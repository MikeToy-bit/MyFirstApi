using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    // 用户角色服务接口 
    public interface IT_SYS_UserRoleService
    {
        // 获取所有用户角色
        Task<List<T_SYS_UserRolesModel>> GetUserRoles();
        // 根据角色获取用户角色
            Task<List<T_SYS_UserRolesModel>> GetUserRolesByRoleId(string roleId);
        // 根据用户获取用户角色
        Task<List<string>> GetRolesByEmpCode(string empCode);
        // 添加用户角色
        Task<T_SYS_UserRolesModel> AddUserRole(T_SYS_UserRolesModel userRole);
        // 删除用户角色
        Task<bool> DeleteUserRole(string userRoleId);
        // 根据角色删除用户角色
        Task<bool> DeleteUserRolesByRoleId(string roleId);
        // 根据用户删除用户角色
        Task<bool> DeleteUserRolesByEmpCode(string empCode);
    }
    // 用户角色服务实现
    public class T_SYS_UserRoleService : IT_SYS_UserRoleService
    {
        private readonly TestDbContext _context;

        public T_SYS_UserRoleService(TestDbContext context)
        {
            _context = context;
        }
        // 获取所有用户角色
        public async Task<List<T_SYS_UserRolesModel>> GetUserRoles()
        {
            return await _context.T_SYS_UserRoles.ToListAsync();
        }
        // 根据角色获取用户角色
        public async Task<List<T_SYS_UserRolesModel>> GetUserRolesByRoleId(string roleId)
        {
            return await _context.T_SYS_UserRoles
                .Where(rm => rm.RoleId == roleId)
                .ToListAsync();
        }
        // 根据用户获取用户角色
        public async Task<List<string>> GetRolesByEmpCode(string empCode)
        {
            return await _context.T_SYS_UserRoles
                .Where(rm => rm.EmpCode == empCode).Select(m=>m.RoleId)
                .ToListAsync();
        }
        // 添加用户角色 
        public async Task<T_SYS_UserRolesModel> AddUserRole(T_SYS_UserRolesModel userRole)
        {
            _context.T_SYS_UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }
        // 删除用户角色
        public async Task<bool> DeleteUserRole(string userRoleId)
        {
            var userRole = await _context.T_SYS_UserRoles.FindAsync(userRoleId);
            if (userRole == null)
            {
                return false;
            }
                _context.T_SYS_UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }
        // 根据角色删除用户角色
        public async Task<bool> DeleteUserRolesByRoleId(string roleId)
        {
            var userRoles = await _context.T_SYS_UserRoles
                .Where(rm => rm.RoleId == roleId)
                .ToListAsync();
            _context.T_SYS_UserRoles.RemoveRange(userRoles);
            await _context.SaveChangesAsync();
            return true;
        }
        // 根据用户删除用户角色 
        public async Task<bool> DeleteUserRolesByEmpCode(string empCode)
        {
            var userRoles = await _context.T_SYS_UserRoles
                .Where(rm => rm.EmpCode == empCode)
                .ToListAsync();
            _context.T_SYS_UserRoles.RemoveRange(userRoles);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 