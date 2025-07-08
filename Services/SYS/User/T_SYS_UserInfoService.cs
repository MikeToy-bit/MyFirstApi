using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;
using MyFirstApi.Extensions;

namespace MyFirstApi.Services
{
    public class T_SYS_userInfoService : IT_SYS_UserInfoService
    {
        private readonly TestDbContext _context;

        public T_SYS_userInfoService(TestDbContext context)
        {
            _context = context;
        }
        // 获取用户信息
        public async Task<List<T_SYS_UserModel>> GetUserInfo()
        {
            return await _context.T_SYS_User.Where(u => u.IsDeleted != true).ToListAsync();
        }

        // 分页查询用户信息（支持多条件筛选）- 使用反射动态查询
        public async Task<(List<UserResponseDTO> Items, int TotalCount)> QueryUsersAsync(UserQueryDTO queryDTO)
        {
            // var baseQuery = _context.T_SYS_User
            //     .Where(u => u.IsDeleted != true)
            //     .ApplyFiltersFromDTO<T_SYS_UserModel, UserQueryDTO>(queryDTO)
            //     .WhereDateRange(queryDTO.StartCreateTime, queryDTO.EndCreateTime, u => u.CreateTime);


            var baseQuery = _context.T_SYS_User
               .Where(u => u.IsDeleted != true)
               .WhereDateRange(queryDTO.StartCreateTime, queryDTO.EndCreateTime, u => u.CreateTime);


            #region 查询条件
            if (!string.IsNullOrEmpty(queryDTO.EmpCode))
            {
                baseQuery = baseQuery.Where(u => u.EmpCode.Equals(queryDTO.EmpCode));
            }

            if (!string.IsNullOrEmpty(queryDTO.EmpName))
            {
                baseQuery = baseQuery.Where(u => u.EmpName.Contains(queryDTO.EmpName));
            }
            #endregion

            var totalCount = await baseQuery.CountAsync();
            var items = await baseQuery
                .OrderByDescending(u => u.CreateTime)
                .Skip((queryDTO.PageIndex - 1) * queryDTO.PageSize)
                .Take(queryDTO.PageSize)
                .Select(u => MapToUserResponseDTO(u))
                .ToListAsync();

            return (items, totalCount);
        }

        // 添加用户信息
        public async Task<T_SYS_UserModel> AddUserInfo(UserCreateRequestDTO userDto, string createEmpCode, string createEmpName)
        {
            // 检查员工编号是否已存在
            var existingUser = await _context.T_SYS_User
                .FirstOrDefaultAsync(u => u.EmpCode == userDto.EmpCode);

            if (existingUser != null)
            {
                throw new BusinessException("员工编号已存在");
            }

            var userInfo = new T_SYS_UserModel
            {
                EmpCode = userDto.EmpCode,
                EmpName = userDto.EmpName,
                Password = "123456", // 实际项目中需要加密
                OrgCode = userDto.OrgCode,
                OrgName = userDto.OrgName,
                PostCode = userDto.PostCode,
                PostName = userDto.PostName,
                Sex = userDto.Sex,
                IdType = userDto.IdType,
                IdCard = userDto.IdCard,
                Birthday = userDto.Birthday,
                PhoneNumber = userDto.PhoneNumber,
                CreateEmpCode = createEmpCode,
                CreateEmpName = createEmpName,
                CreateTime = DateTime.Now,
                ModifyEmpCode = createEmpCode,
                ModifyEmpName = createEmpName,
                ModifyTime = DateTime.Now,
                IsDeleted = false
            };

            _context.T_SYS_User.Add(userInfo);
            await _context.SaveChangesAsync();
            return userInfo;
        }

        // 更新用户信息
        public async Task<bool> UpdateUserInfo(UserUpdateRequestDTO userDto, string modifyEmpCode, string modifyEmpName)
        {
            var userInfo = await _context.T_SYS_User
                .FirstOrDefaultAsync(u => u.EmpCode == userDto.EmpCode && u.IsDeleted != true);

            if (userInfo == null)
            {
                return false;
            }

            // 更新字段
            userInfo.EmpName = userDto.EmpName;
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                userInfo.Password = userDto.Password; // 实际项目中需要加密
            }
            userInfo.OrgCode = userDto.OrgCode;
            userInfo.OrgName = userDto.OrgName;
            userInfo.PostCode = userDto.PostCode;
            userInfo.PostName = userDto.PostName;
            userInfo.Sex = userDto.Sex;
            userInfo.IdType = userDto.IdType;
            userInfo.IdCard = userDto.IdCard;
            userInfo.Birthday = userDto.Birthday;
            userInfo.PhoneNumber = userDto.PhoneNumber;
            userInfo.ModifyEmpCode = modifyEmpCode;
            userInfo.ModifyEmpName = modifyEmpName;
            userInfo.ModifyTime = DateTime.Now;

            _context.T_SYS_User.Update(userInfo);
            return await _context.SaveChangesAsync() > 0;
        }

        // 逻辑删除用户信息
        public async Task<bool> DeleteUserInfo(string empCode, string deleteEmpCode, string deleteEmpName)
        {
            var userInfo = await _context.T_SYS_User
                .FirstOrDefaultAsync(u => u.EmpCode == empCode && u.IsDeleted != true);

            if (userInfo == null)
            {
                return false;
            }

            userInfo.IsDeleted = true;
            userInfo.ModifyEmpCode = deleteEmpCode;
            userInfo.ModifyEmpName = deleteEmpName;
            userInfo.ModifyTime = DateTime.Now;

            _context.T_SYS_User.Update(userInfo);
            return await _context.SaveChangesAsync() > 0;
        }

        // 批量删除用户信息
        public async Task<bool> BatchDeleteUsers(List<string> empCodes, string deleteEmpCode, string deleteEmpName)
        {
            if (empCodes == null || !empCodes.Any())
            {
                return false;
            }

            var users = await _context.T_SYS_User
                .Where(u => empCodes.Contains(u.EmpCode) && u.IsDeleted != true)
                .ToListAsync();

            foreach (var user in users)
            {
                user.IsDeleted = true;
                user.ModifyEmpCode = deleteEmpCode;
                user.ModifyEmpName = deleteEmpName;
                user.ModifyTime = DateTime.Now;
            }

            _context.T_SYS_User.UpdateRange(users);
            return await _context.SaveChangesAsync() > 0;
        }

        // 恢复已删除的用户
        public async Task<bool> RestoreUser(string empCode, string restoreEmpCode, string restoreEmpName)
        {
            var userInfo = await _context.T_SYS_User
                .FirstOrDefaultAsync(u => u.EmpCode == empCode && u.IsDeleted == true);

            if (userInfo == null)
            {
                return false;
            }

            userInfo.IsDeleted = false;
            userInfo.ModifyEmpCode = restoreEmpCode;
            userInfo.ModifyEmpName = restoreEmpName;
            userInfo.ModifyTime = DateTime.Now;

            _context.T_SYS_User.Update(userInfo);
            return await _context.SaveChangesAsync() > 0;
        }

        // 根据员工编号获取用户信息
        public async Task<UserResponseDTO?> GetUserInfoByEmpCode(string empCode)
        {
            var user = await _context.T_SYS_User
                .Where(u => u.EmpCode == empCode && u.IsDeleted != true)
                .Select(u => MapToUserResponseDTO(u))
                .FirstOrDefaultAsync();
            return user;
        }

        // 根据员工编号获取密码信息
        public async Task<string> GetPasswordByEmpCode(string empCode)
        {
            var user = await _context.T_SYS_User
                .Where(u => u.EmpCode == empCode && u.IsDeleted != true)
                .Select(u => u.Password)
                .FirstOrDefaultAsync();
            return user ?? string.Empty;
        }

        /// <summary>
        /// 将用户实体映射为响应DTO
        /// </summary>
        private static UserResponseDTO MapToUserResponseDTO(T_SYS_UserModel user)
        {
            return new UserResponseDTO
            {
                EmpCode = user.EmpCode,
                EmpName = user.EmpName,
                OrgCode = user.OrgCode,
                OrgName = user.OrgName,
                PostCode = user.PostCode,
                PostName = user.PostName,
                Sex = user.Sex,
                IdType = user.IdType,
                IdCard = user.IdCard,
                Birthday = user.Birthday,
                PhoneNumber = user.PhoneNumber,
                CreateEmpCode = user.CreateEmpCode,
                CreateEmpName = user.CreateEmpName,
                CreateTime = user.CreateTime,
                ModifyEmpCode = user.ModifyEmpCode,
                ModifyEmpName = user.ModifyEmpName,
                ModifyTime = user.ModifyTime
            };
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