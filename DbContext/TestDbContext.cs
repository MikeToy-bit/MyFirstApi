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
        public DbSet<T_SYS_FilesModel> T_SYS_Files { get; set; }
        public DbSet<T_SYS_DictionaryModel> T_SYS_Dictionary { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置用户与用户角色关系
            modelBuilder.Entity<T_SYS_UserRolesModel>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.EmpCode);
            
            // 配置角色与用户角色关系
            modelBuilder.Entity<T_SYS_UserRolesModel>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            
            // 配置角色与角色菜单关系
            modelBuilder.Entity<T_SYS_RoleMenusModel>()
                .HasOne(rm => rm.Role)
                .WithMany(r => r.RoleMenus)
                .HasForeignKey(rm => rm.RoleId);
            
            // 配置菜单与角色菜单关系
            modelBuilder.Entity<T_SYS_RoleMenusModel>()
                .HasOne(rm => rm.Menu)
                .WithMany(m => m.RoleMenus)
                .HasForeignKey(rm => rm.MenuId);
        }
    }
}    