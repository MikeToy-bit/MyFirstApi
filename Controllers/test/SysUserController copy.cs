using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services;

namespace MyWebApi.Controllers
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

        [HttpGet]
        public async Task<ActionResult<List<T_sys_test>>> GetUserInfo()
        {
            var testData = await _userService.GetUserInfo();
            return Ok(testData);
        }

        [HttpPost]

        public async Task<ActionResult<T_SYS_UserModel>> AddUserInfo(T_SYS_UserModel UserInfo)
        {
         var userInfo=  await  _userService.AddUserInfo(UserInfo);
            return CreatedAtAction(nameof(GetUserInfo), new { EmpCode = userInfo.EmpCode}, userInfo);
            
        }
    }
}    