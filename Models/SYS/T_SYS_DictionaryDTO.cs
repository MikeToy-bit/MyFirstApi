using System;
using System.Collections.Generic;

namespace MyFirstApi.Models
{
    // 创建请求DTO
    public class DictionaryCreateRequestDTO
    {
        public string DictCode { get; set; }
        public string DictName { get; set; }
        public string DictValue { get; set; }
        public string ParentId { get; set; }
        public int DictLevel { get; set; } = 1;
        public int DictOrder { get; set; } = 0;
        public string DictType { get; set; }
        public string DictDesc { get; set; }
        public byte Status { get; set; } = 1;
    }

    // 更新请求DTO
    public class DictionaryUpdateRequestDTO
    {
        public string DictId { get; set; }
        public string DictCode { get; set; }
        public string DictName { get; set; }
        public string DictValue { get; set; }
        public string ParentId { get; set; }
        public int DictLevel { get; set; }
        public int DictOrder { get; set; }
        public string DictType { get; set; }
        public string DictDesc { get; set; }
        public byte Status { get; set; }
    }

    // 查询参数DTO
    public class DictionaryQueryDTO
    {
        public string DictCode { get; set; }
        public string DictName { get; set; }
        public string DictType { get; set; }
        public string ParentId { get; set; }
        public byte? Status { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // 树形结构响应DTO
    public class DictionaryTreeNodeDTO
    {
        public string DictId { get; set; }
        public string DictCode { get; set; }
        public string DictName { get; set; }
        public string DictValue { get; set; }
        public string ParentId { get; set; }
        public int DictLevel { get; set; }
        public int DictOrder { get; set; }
        public string DictType { get; set; }
        public string DictDesc { get; set; }
        public byte Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public List<DictionaryTreeNodeDTO> Children { get; set; } = new List<DictionaryTreeNodeDTO>();
    }
} 