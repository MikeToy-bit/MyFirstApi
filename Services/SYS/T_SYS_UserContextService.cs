using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MyFirstApi.Services
{
    /// <summary>
    /// 用户上下文服务实现，用于获取当前登录用户信息
    /// </summary>
    public class T_SYS_UserContextService : IT_SYS_UserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public T_SYS_UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取当前登录用户工号
        /// </summary>
        public string GetCurrentEmpCode()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
        }

        /// <summary>
        /// 获取当前登录用户姓名
        /// </summary>
        public string GetCurrentEmpName()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst(ClaimTypes.Name)?.Value ?? "系统用户";
        }
        /// <summary>
        /// 获取当前登录用户组织编码
        /// </summary>
        /// <returns></returns>
        public string GetCurrentOrgCode()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst("OrgCode")?.Value ?? string.Empty;
        }   

        /// <summary>
        /// 获取当前登录用户组织名称
        /// </summary>
        /// <returns></returns>
        public string GetCurrentOrgName()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst("OrgName")?.Value ?? string.Empty;
        }   

        /// <summary>
        /// 获取当前登录用户岗位编码
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPostCode()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst("PostCode")?.Value ?? string.Empty;
        }   

        /// <summary>
        /// 获取当前登录用户岗位名称
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPostName()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst("PostName")?.Value ?? string.Empty;
        }   
        /// <summary>
        /// 获取当前登录用户性别
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSex()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst("Sex")?.Value ?? string.Empty;
        }   
        /// <summary>
        /// 获取当前登录用户生日
        /// </summary>
        /// <returns></returns>
        public string GetCurrentBirthday()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst("Birthday")?.Value ?? string.Empty;
        }   
        /// <summary>
        /// 获取当前登录用户手机号码
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPhoneNumber()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst("PhoneNumber")?.Value ?? string.Empty;
        }
        /// <summary>
        /// 同时获取当前登录用户的工号和姓名
        /// </summary>
        public (string empCode, string empName) GetCurrentUser()
        {
            return (GetCurrentEmpCode(), GetCurrentEmpName());
        }
    }
} 