using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public class T_sys_userInfoService
    {
        private readonly TestDbContext _context;

        public T_sys_userInfoService(TestDbContext context)
        {
            _context = context;
        }
        // 获取用户信息
        public async Task<List<T_SYS_UserModel>> GetUserInfo()
        {
            return await _context.T_sys_user.ToListAsync();
        }
        // 添加用户信息
        public async Task<T_SYS_UserModel> AddUserInfo(T_SYS_UserModel userInfo)
        {
            _context.T_sys_user.Add(userInfo);
            await _context.SaveChangesAsync();
            return userInfo;
        }
        // 更新用户信息
        public async Task<T_SYS_UserModel> UpdateUserInfo(T_SYS_UserModel userInfo)
        {
            _context.T_sys_user.Update(userInfo);
            await _context.SaveChangesAsync();
            return userInfo;
        }
        // 删除用户信息
        public async Task<bool> DeleteUserInfo(string empCode)
        {
            var userInfo = await _context.T_sys_user.FindAsync(empCode);
            if (userInfo == null)
            {
                return false;   
            }
            _context.T_sys_user.Remove(userInfo);
            await _context.SaveChangesAsync();
            return true;
        }
        // 根据员工编号获取用户信息
        public async Task<T_SYS_UserModel?> GetUserInfoByEmpCode(string empCode)
        {
            return await _context.T_sys_user.FindAsync(empCode);
        }   

    }
}    