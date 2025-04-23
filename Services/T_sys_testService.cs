using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public class T_sys_testService
    {
        private readonly TestDbContext _context;

        public T_sys_testService(TestDbContext context)
        {
            _context = context;
        }

        // 获取测试数据
        public async Task<List<T_sys_test>> GetTestData()
        {
            return await _context.T_SYS_Test.ToListAsync();
        }
    }
}    