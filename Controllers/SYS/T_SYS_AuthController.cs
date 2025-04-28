using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class T_SYS_AuthController : ControllerBase
    {
        private readonly IT_SYS_AuthService _authService;
        private readonly T_SYS_userInfoService _userService;

        public T_SYS_AuthController(IT_SYS_AuthService authService, T_SYS_userInfoService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] T_SYS_UserModel user)
        {
            try
            {
                // 验证用户
                var userInfo = await _userService.GetUserInfoByEmpCode(user.EmpCode);
                if (userInfo == null)
                {
                    return BadRequest(new ApiResponse<object>(400, "用户不存在"));
                }

                // 验证密码哈希
                // if (!_authService.VerifyPassword(user.Password, userInfo.Password))
                // {
                //     return BadRequest(new ApiResponse<object>(400, "密码错误"));
                // }

                // 生成Token
                var tokens = _authService.GenerateTokens(userInfo);
                return Ok(new ApiResponse<object>(tokens));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var tokens = _authService.RefreshAccessToken(refreshToken);
                return Ok(new ApiResponse<object>(tokens));
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new ApiResponse<object>(401, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpGet("validate")]
        [Authorize]
        // 验证Token
        public IActionResult ValidateToken()
        {
            try
            {
                return Ok(new ApiResponse<object>(new { isValid = true }));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                await _authService.InvalidateToken(token);
                return Ok(new ApiResponse<object>(new { message = "登出成功" }));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }
    }
} 