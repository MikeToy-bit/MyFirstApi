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

        string GetCurrentOrgCode();

        string GetCurrentOrgName();

        string GetCurrentPostCode();

        string GetCurrentPostName();    

        string GetCurrentSex();

        string GetCurrentBirthday();

        string GetCurrentPhoneNumber(); 
        /// <summary>
        /// 同时获取当前登录用户的工号和姓名
        /// </summary>
        (string empCode, string empName) GetCurrentUser();
    }
} 