using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using MyFirstApi.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class T_SYS_UserController : ControllerBase
    {
        private readonly T_SYS_userInfoService _userService;
        private readonly IT_SYS_UserContextService _userContextService;

        public T_SYS_UserController(T_SYS_userInfoService userInfoService, IT_SYS_UserContextService userContextService)
        {
            _userService = userInfoService;
            _userContextService = userContextService;
        }

        /// <summary>
        /// 获取用户信息（简单列表）
        /// </summary>
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

        /// <summary>
        /// 分页查询用户信息（支持多条件筛选）
        /// </summary>
        [HttpPost("query")]
        public async Task<IActionResult> QueryUsers([FromBody] UserQueryDTO queryDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(ApiResponse<object>.BadRequest("请求参数无效"));
                }

                var (dataInfo, totalCount) = await _userService.QueryUsersAsync(queryDTO);
                
                var result = new
                {
                    DataInfo = dataInfo,
                    TotalCount = totalCount,
                    PageIndex = queryDTO.PageIndex,
                    PageSize = queryDTO.PageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)queryDTO.PageSize)
                };

                return Ok(ApiResponse<object>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 根据员工编号获取用户详情
        /// </summary>
        [HttpGet("{empCode}")]
        public async Task<IActionResult> GetUserByEmpCode(string empCode)
        {
            try
            {
                if (string.IsNullOrEmpty(empCode))
                {
                    return Ok(ApiResponse<object>.BadRequest("员工编号不能为空"));
                }

                var user = await _userService.GetUserInfoByEmpCode(empCode);
                if (user == null)
                {
                    return Ok(ApiResponse<object>.NotFound("用户不存在"));
                }

                return Ok(ApiResponse<UserResponseDTO>.Ok(user));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDTO userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(ApiResponse<object>.BadRequest("请求参数无效"));
                }

                var (empCode, empName) = _userContextService.GetCurrentUser();
                var user = await _userService.AddUserInfo(userDto, empCode, empName);
                
                return Ok(ApiResponse<T_SYS_UserModel>.Ok(user, "用户创建成功"));
            }
            catch (BusinessException ex)
            {
                return Ok(ApiResponse<object>.BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequestDTO userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(ApiResponse<object>.BadRequest("请求参数无效"));
                }

                var (empCode, empName) = _userContextService.GetCurrentUser();
                var result = await _userService.UpdateUserInfo(userDto, empCode, empName);
                
                if (!result)
                {
                    return Ok(ApiResponse<object>.NotFound("用户不存在"));
                }

                return Ok(ApiResponse<object>.Ok(result, "用户更新成功"));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 删除用户（逻辑删除）
        /// </summary>
        [HttpDelete("{empCode}")]
        public async Task<IActionResult> DeleteUser(string empCode)
        {
            try
            {
                if (string.IsNullOrEmpty(empCode))
                {
                    return Ok(ApiResponse<object>.BadRequest("员工编号不能为空"));
                }

                var (deleteEmpCode, deleteEmpName) = _userContextService.GetCurrentUser();
                var result = await _userService.DeleteUserInfo(empCode, deleteEmpCode, deleteEmpName);
                
                if (!result)
                {
                    return Ok(ApiResponse<object>.NotFound("用户不存在"));
                }

                return Ok(ApiResponse<object>.Ok(result, "用户删除成功"));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        [HttpPost("batch-delete")]
        public async Task<IActionResult> BatchDeleteUsers([FromBody] List<string> empCodes)
        {
            try
            {
                if (empCodes == null || !empCodes.Any())
                {
                    return Ok(ApiResponse<object>.BadRequest("员工编号列表不能为空"));
                }

                var (deleteEmpCode, deleteEmpName) = _userContextService.GetCurrentUser();
                var result = await _userService.BatchDeleteUsers(empCodes, deleteEmpCode, deleteEmpName);
                
                if (!result)
                {
                    return Ok(ApiResponse<object>.BadRequest("批量删除失败"));
                }

                return Ok(ApiResponse<object>.Ok(result, "批量删除成功"));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 恢复已删除的用户
        /// </summary>
        [HttpPost("restore/{empCode}")]
        public async Task<IActionResult> RestoreUser(string empCode)
        {
            try
            {
                if (string.IsNullOrEmpty(empCode))
                {
                    return Ok(ApiResponse<object>.BadRequest("员工编号不能为空"));
                }

                var (restoreEmpCode, restoreEmpName) = _userContextService.GetCurrentUser();
                var result = await _userService.RestoreUser(empCode, restoreEmpCode, restoreEmpName);
                
                if (!result)
                {
                    return Ok(ApiResponse<object>.NotFound("用户不存在或未被删除"));
                }

                return Ok(ApiResponse<object>.Ok(result, "用户恢复成功"));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 根据员工编号获取菜单
        /// </summary>
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