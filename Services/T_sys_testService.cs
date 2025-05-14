using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;
using Microsoft.Extensions.Logging;

namespace MyFirstApi.Services
{
    public class T_sys_testService
    {
        private readonly TestDbContext _context;
        private readonly ILogger<T_sys_testService> _logger;

        public T_sys_testService(TestDbContext context, ILogger<T_sys_testService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 获取测试数据
        public async Task<List<T_sys_test>> GetTestData()
        {
            try
            {
                _logger.LogInformation("开始获取测试数据");
                var result = await _context.T_SYS_Test.ToListAsync();
                _logger.LogInformation($"成功获取{result.Count}条测试数据");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取测试数据时发生错误");
                throw;
            }
        }
    }
}    