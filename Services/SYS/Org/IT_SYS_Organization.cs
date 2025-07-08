using MyFirstApi.Models;
using System.Collections.Generic;

namespace MyFirstApi.Services
{
    public interface IT_SYS_Organization
    {
        //根据目标组织编码获取子节点
        Task<List<GetOrgListByTargetOrgMgCodeDTO>> GetOrgListByTargetOrgMgCode(string targetOrgMgCode);
        //新增组织信息
        Task<T_SYS_OrganizationModel> AddOrg(T_SYS_OrganizationModel org);
        //更新组织信息
        Task<T_SYS_OrganizationModel> UpdateOrg(T_SYS_OrganizationModel org);
        //删除组织信息
        Task<bool> DeleteOrg(string orgCode);
        //根据组织编码获取组织信息
        Task<T_SYS_OrganizationModel?> GetOrgByOrgCode(string orgCode);
    }

}