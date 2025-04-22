using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;
using MyWebApi.Models;

namespace MyWebApi.Services
{
    public class T_sys_testService
    {
        private readonly TestDbContext _context;

        public T_sys_testService(TestDbContext context)
        {
            _context = context;
        }

        public async Task<List<T_sys_test>> GetTestData()
        {
            return await _context.T_SYS_Test.ToListAsync();
        }
    }
}    