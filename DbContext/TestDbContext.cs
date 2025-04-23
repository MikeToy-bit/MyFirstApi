using Microsoft.EntityFrameworkCore;
using MyFirstApi.Models;

namespace MyFirstApi.Data
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<T_sys_test> T_SYS_Test { get; set; }
        public DbSet<T_SYS_UserModel> T_sys_user { get; set; }
    }
}    