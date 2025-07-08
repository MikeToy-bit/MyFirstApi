using System.Collections.Generic;
using System.Threading.Tasks;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public interface IT_SYS_UserInfoService
    {
        // 获取用户信息
        Task<List<T_SYS_UserModel>> GetUserInfo();
        
        // 分页查询用户信息（支持多条件筛选）
        Task<(List<UserResponseDTO> Items, int TotalCount)> QueryUsersAsync(UserQueryDTO queryDTO);
        
        // 添加用户信息
        Task<T_SYS_UserModel> AddUserInfo(UserCreateRequestDTO userDto, string createEmpCode, string createEmpName);
        
        // 更新用户信息
        Task<bool> UpdateUserInfo(UserUpdateRequestDTO userDto, string modifyEmpCode, string modifyEmpName);
        
        // 逻辑删除用户信息
        Task<bool> DeleteUserInfo(string empCode, string deleteEmpCode, string deleteEmpName);
        
        // 批量删除用户信息
        Task<bool> BatchDeleteUsers(List<string> empCodes, string deleteEmpCode, string deleteEmpName);
        
        // 恢复已删除的用户
        Task<bool> RestoreUser(string empCode, string restoreEmpCode, string restoreEmpName);
        
        // 根据员工编号获取用户信息
        Task<UserResponseDTO?> GetUserInfoByEmpCode(string empCode);
        
        // 根据员工编号获取用户菜单
        Task<List<T_SYS_RetUserMenusModel>> GetMenusByEmpCode(string empCode);

        // 根据工号获取密码
        Task<string> GetPasswordByEmpCode(string empCode);
    }
} 