using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Models
{
    // 定义一个T_SYS_User类
    // 定义一个T_SYS_User类
    public class T_SYS_UserModel
    {
        // 员工姓名
        [Key]
        public required  string EmpCode { get; set; }
        // 组织编号
        public required string EmpName { get; set; }
        // 密码
        public required string Password { get; set; }
        // 组织名称
        public required string OrgCode { get; set; }
        public required string OrgName { get; set; }
        // 岗位编号

        public required string PostCode { get; set; }
        // 岗位名称

        public required string PostName { get; set; }
        // 性别

        public required bool Sex { get; set; }
        // 身份证类型

        public required string IdType { get; set; }
        // 身份证号码

        public required string IdCard { get; set; }
        // 出生日期

        public required string Birthday { get; set; }
        // 电话号码

        public string? PhoneNumber { get; set; }
        // 创建人编号

        public  string? CreateEmpCode { get; set; }
        // 创建人姓名

        public string? CreateEmpName { get; set; }
        // 创建时间

        public string? CreateTime { get; set; }
        // 修改人编号

        public string? ModifyEmpCode { get; set; }
        // 修改人姓名

        public string? ModifyEmpName { get; set; }
        // 修改时间

        public string? ModifyTime { get; set; }
        // 是否删除

        public bool? IsDeleted { get; set; }
    }
}    