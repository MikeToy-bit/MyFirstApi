using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstApi.Models
{
    [Table("T_SYS_Dictionary")]
    public class T_SYS_DictionaryModel
    {
        [Key]
        [Column("DictId")]
        [StringLength(32)]
        public string ?DictId { get; set; }

        [Required]
        [Column("DictCode")]
        [StringLength(50)]
        public string ?DictCode { get; set; }

        [Required]
        [Column("DictName")]
        [StringLength(100)]
        public string ?DictName { get; set; }

        [Column("DictValue")]
        [StringLength(255)]
        public string ?DictValue { get; set; }

        [Column("ParentId")]
        [StringLength(32)]
        public string ?ParentId { get; set; }

        [Required]
        [Column("DictLevel")]
        public int DictLevel { get; set; } = 1;

        [Required]
        [Column("DictOrder")]
        public int DictOrder { get; set; } = 0;

        [Required]
        [Column("DictType")]
        [StringLength(32)]
        public string ?DictType { get; set; }

        [Column("DictDesc")]
        [StringLength(255)]
        public string ?DictDesc { get; set; }

        [Required]
        [Column("Status")]
        public byte Status { get; set; } = 1;

        [Column("CreateEmpCode")]
        [StringLength(50)]
        public string ?CreateEmpCode { get; set; }

        [Column("CreateEmpName")]
        [StringLength(50)]
        public string ?CreateEmpName { get; set; }

        [Column("CreateTime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [Column("ModifyEmpCode")]
        [StringLength(50)]
        public string ?ModifyEmpCode { get; set; }

        [Column("ModifyEmpName")]
        [StringLength(50)]
        public string ?ModifyEmpName { get; set; }

        [Column("ModifyTime")]
        public DateTime ModifyTime { get; set; } = DateTime.Now;

        [Required]
        [Column("IsDeleted")]
        public bool IsDeleted { get; set; } = false;

        // 添加导航属性，用于树形结构
        [NotMapped]
        public List<T_SYS_DictionaryModel> Children { get; set; } = new List<T_SYS_DictionaryModel>();
    }
} 