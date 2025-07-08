using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public class T_SYS_Organization : IT_SYS_Organization
    {
        private readonly TestDbContext _context;

        public T_SYS_Organization(TestDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetOrgListByTargetOrgMgCodeDTO>> GetOrgListByTargetOrgMgCode(string targetOrgMgCode)
        {
            try
            {
                var orgList = await _context.T_SYS_Organization.Where(o => o.TargetOrgMgCode == targetOrgMgCode && o.IsDeleted == false).ToListAsync();
                return orgList.Select(o => new GetOrgListByTargetOrgMgCodeDTO
                {
                    Id = o.OrgCode,
                    Label = o.OrgName ?? "",
                    Leaf = !_context.T_SYS_Organization.Any(c => c.TargetOrgMgCode == o.OrgCode)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T_SYS_OrganizationModel> AddOrg(T_SYS_OrganizationModel org)
        {
            try
            {
                _context.T_SYS_Organization.Add(org);
                await _context.SaveChangesAsync();
                return org;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T_SYS_OrganizationModel> UpdateOrg(T_SYS_OrganizationModel org)
        {
            try
            {
                _context.T_SYS_Organization.Update(org);
                await _context.SaveChangesAsync();
                return org;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteOrg(string orgCode)
        {
            try
            {
                var org = await _context.T_SYS_Organization.FindAsync(orgCode);
                if (org == null)
                {
                    return false;
                }
                _context.T_SYS_Organization.Remove(org);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T_SYS_OrganizationModel?> GetOrgByOrgCode(string orgCode)
        {
            try
            {
                if (_context.T_SYS_Organization.Any(o => o.OrgCode == orgCode))
                {
                    return await _context.T_SYS_Organization.FirstOrDefaultAsync(o => o.OrgCode == orgCode);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}