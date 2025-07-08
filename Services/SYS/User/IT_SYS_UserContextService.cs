namespace MyFirstApi.Services
{
    /// <summary>
    /// 用户上下文服务接口，用于获取当前登录用户信息
    /// </summary>
    public interface IT_SYS_UserContextService
    {
        /// <summary>
        /// 获取当前登录用户工号
        /// </summary>
        string GetCurrentEmpCode();
        
        /// <summary>
        /// 获取当前登录用户姓名
        /// </summary>
        string GetCurrentEmpName();
        /// <summary>
        /// 获取当前登录用户所属组织编码
        /// </summary>
        /// <returns></returns>
        string GetCurrentOrgCode();
        /// <summary>
        /// 获取当前登录用户所属组织名称
        /// </summary>
        /// <returns></returns>
        string GetCurrentOrgName();
        /// <summary>
        /// 获取当前登录用户所属岗位编码
        /// </summary>
        /// <returns></returns>
        string GetCurrentPostCode();
        /// <summary>
        /// 获取当前登录用户所属岗位名称
        /// </summary>
        /// <returns></returns>
        string GetCurrentPostName();
        /// <summary>
        /// 获取当前登录用户性别
        /// </summary>
        /// <returns></returns>
        string GetCurrentSex();
        /// <summary>
        /// 获取当前登录用户生日
        /// </summary>
        /// <returns></returns>
        string GetCurrentBirthday();
        /// <summary>
        /// 获取当前登录用户手机号码
        /// </summary>
        /// <returns></returns>
        string GetCurrentPhoneNumber(); 
        /// <summary>
        /// 同时获取当前登录用户的工号和姓名
        /// </summary>
        (string empCode, string empName) GetCurrentUser();
    }
} 