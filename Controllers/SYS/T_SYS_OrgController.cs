using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/org")]
    [Authorize]
    public class T_SYS_OrgController : ControllerBase
    {
        private readonly IT_SYS_Organization _orgService;

        public T_SYS_OrgController(IT_SYS_Organization orgService)
        {
            _orgService = orgService;
        }

        [HttpGet]
        public async Task<ApiResponse<object>> GetOrgListByTargetOrgMgCode(string targetOrgMgCode)
        {
            try
            {
                var orgList = await _orgService.GetOrgListByTargetOrgMgCode(targetOrgMgCode);
                return ApiResponse<object>.Ok(orgList);
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Error(ex.Message);
            }
        }
    }


}