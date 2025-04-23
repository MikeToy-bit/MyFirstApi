using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;
using MyWebApi.Models;

namespace MyWebApi.Services
{
    public class T_sys_userInfoService
    {
        private readonly TestDbContext _context;

        public T_sys_userInfoService(TestDbContext context)
        {
            _context = context;
        }

        public async Task<List<T_SYS_UserModel>> GetUserInfo()
        {
            return await _context.T_sys_user.ToListAsync();
        }

        public async Task<T_SYS_UserModel> AddUserInfo(T_SYS_UserModel userInfo)
        {
            _context.T_sys_user.Add(userInfo);
            await _context.SaveChangesAsync();
            return userInfo;
        }
    }
}    