using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using MyFirstApi.Services;
using System.Security.Claims;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class T_SYS_UserController : ControllerBase
    {
        private readonly T_SYS_userInfoService _userService;

        public T_SYS_UserController(T_SYS_userInfoService userInfoService)
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

        // 根据员工编号获取菜单
        [HttpGet("getMenusByToken")] 
        public async Task<IActionResult> GetMenusByToken()
        {
            try
            {
                // 从当前用户的Claims中获取工号
                var empCode = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                // 如果找不到工号，返回未授权
                if (string.IsNullOrEmpty(empCode))
                {
                    return Unauthorized("未找到有效的用户信息");
                }
                
                var menus = await _userService.GetMenusByEmpCode(empCode);
                return Ok(new ApiResponse<object>(menus));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(500, ex.Message));
            }
        }
    }
}    