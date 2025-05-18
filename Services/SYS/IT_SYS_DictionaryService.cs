using System.Collections.Generic;
using System.Threading.Tasks;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public interface IT_SYS_DictionaryService
    {
        // 创建数据字典
        Task<T_SYS_DictionaryModel> CreateDictionaryAsync(DictionaryCreateRequestDTO createDTO);
        
        // 更新数据字典
        Task<bool> UpdateDictionaryAsync(DictionaryUpdateRequestDTO updateDTO);
        
        // 删除数据字典（逻辑删除）
        Task<bool> DeleteDictionaryAsync(string dictId);
        
        // 获取字典详情
        Task<T_SYS_DictionaryModel?> GetDictionaryByIdAsync(string dictId);
        
        // 根据条件查询字典（分页）
        Task<(List<T_SYS_DictionaryModel> Items, int TotalCount)> QueryDictionariesAsync(DictionaryQueryDTO queryDTO);
        
        // 根据字典类型获取树形结构
        Task<List<DictionaryTreeNodeDTO>> GetDictionaryTreeByTypeAsync(string dictType);
        
        // 获取指定节点的所有子节点（包含子节点的子节点）
        Task<List<T_SYS_DictionaryModel>> GetAllChildrenAsync(string parentId);
        
        // 批量删除（逻辑删除）
        Task<bool> BatchDeleteAsync(List<string> dictIds);
    }
} 