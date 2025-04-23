using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class T_SYS_AuthController : ControllerBase
    {
        private readonly IT_SYS_AuthService _authService;

        public T_SYS_AuthController(IT_SYS_AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] T_SYS_UserModel user)
        {
            try
            {
                // TODO: 这里需要添加实际的用户验证逻辑
                // 示例中直接生成token
                var token = _authService.GenerateToken(user);
                return Ok(new ApiResponse<object>(new { token }));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpGet("validate")]
        [Authorize]
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
    }
} 