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
        public DbSet<T_SYS_UserModel> T_SYS_User { get; set; }
        public DbSet<T_SYS_RolesModel> T_SYS_Roles { get; set; }
        public DbSet<T_SYS_MenusModel> T_SYS_Menus { get; set; }
        public DbSet<T_SYS_RoleMenusModel> T_SYS_RoleMenus { get; set; }
        public DbSet<T_SYS_UserRolesModel> T_SYS_UserRoles { get; set; }
    }
}    