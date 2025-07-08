using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models
{

    public class T_SYS_OrganizationModel
    {
        [Key]
        public required string OrgCode { get; set; }
        public string? OrgName { get; set; }
        public string? OrgBriefName { get; set; }
        public string? SpellHead { get; set; }
        public string? OrgTypeId { get; set; }
        public string? OrgTypeName { get; set; }
        public string? TargetOrgMgCode { get; set; }
        public string? DataStatus { get; set; }
        public string? OutSourceCorpId { get; set; }
        public string? OutSourceCorpName { get; set; }
        public string? CreateEmpCode { get; set; }
        public string? CreateEmpName { get; set; }
        public string? CreateTime { get; set; }
        public string? ModifyEmpCode { get; set; }
        public string? ModifyEmpName { get; set; }
        public string? ModifyTime { get; set; }
        public string? Remark { get; set; }
        public bool? IsDeleted { get; set; }

    }


    public class GetOrgListByTargetOrgMgCodeDTO
    {
        public required string Id { get; set; }
        public required string Label { get; set; }
        public required bool Leaf { get; set; }
    }

}