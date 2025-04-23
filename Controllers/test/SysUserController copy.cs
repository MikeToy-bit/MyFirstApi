using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SysUserController : ControllerBase
    {
        private readonly T_sys_userInfoService _userService;

        public SysUserController(T_sys_userInfoService userInfoService)
        {
            _userService = userInfoService;
        }

        // 获取用户信息
        [HttpGet]
        public async Task<ApiResponse<List<T_SYS_UserModel>>> GetUserInfo()
        {
            try
            {
                var userData = await _userService.GetUserInfo();
                return new ApiResponse<List<T_SYS_UserModel>>(userData);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<T_SYS_UserModel>>(500, ex.Message);
            }
        }

        // 添加用户信息
        [HttpPost]
        public async Task<ApiResponse<T_SYS_UserModel>> AddUserInfo(T_SYS_UserModel UserInfo)
        {
            try
            {
                var userInfo = await _userService.AddUserInfo(UserInfo);
                return new ApiResponse<T_SYS_UserModel>(userInfo);
            }
            catch (Exception ex)
            {
                return new ApiResponse<T_SYS_UserModel>(500, ex.Message);
            }
        }
    }
}    