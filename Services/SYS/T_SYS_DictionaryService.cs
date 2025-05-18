using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Models;
using MyFirstApi.Data;

namespace MyFirstApi.Services
{
    public class T_SYS_DictionaryService : IT_SYS_DictionaryService
    {
        private readonly TestDbContext _context;
        private readonly IT_SYS_UserContextService _userContextService;

        public T_SYS_DictionaryService(TestDbContext context, IT_SYS_UserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        // 创建数据字典
        public async Task<T_SYS_DictionaryModel> CreateDictionaryAsync(DictionaryCreateRequestDTO createDTO)
        {
            var (empCode, empName) = _userContextService.GetCurrentUser();
            
            var dictionary = new T_SYS_DictionaryModel
            {
                DictId = Guid.NewGuid().ToString("N"),
                DictCode = createDTO.DictCode,
                DictName = createDTO.DictName,
                DictValue = createDTO.DictValue,
                ParentId = createDTO.ParentId,
                DictLevel = createDTO.DictLevel,
                DictOrder = createDTO.DictOrder,
                DictType = createDTO.DictType,
                DictDesc = createDTO.DictDesc,
                Status = createDTO.Status,
                CreateEmpCode = empCode,
                CreateEmpName = empName,
                CreateTime = DateTime.Now,
                ModifyEmpCode = empCode,
                ModifyEmpName = empName,
                ModifyTime = DateTime.Now,
                IsDeleted = false
            };

            await _context.T_SYS_Dictionary.AddAsync(dictionary);
            await _context.SaveChangesAsync();

            return dictionary;
        }

        // 更新数据字典
        public async Task<bool> UpdateDictionaryAsync(DictionaryUpdateRequestDTO updateDTO)
        {
            var dictionary = await _context.T_SYS_Dictionary
                .FirstOrDefaultAsync(d => d.DictId == updateDTO.DictId && !d.IsDeleted);

            if (dictionary == null)
            {
                return false;
            }

           var (empCode, empName) = _userContextService.GetCurrentUser();

            dictionary.DictCode = updateDTO.DictCode;
            dictionary.DictName = updateDTO.DictName;
            dictionary.DictValue = updateDTO.DictValue;
            dictionary.ParentId = updateDTO.ParentId;
            dictionary.DictLevel = updateDTO.DictLevel;
            dictionary.DictOrder = updateDTO.DictOrder;
            dictionary.DictType = updateDTO.DictType;
            dictionary.DictDesc = updateDTO.DictDesc;
            dictionary.Status = updateDTO.Status;
            dictionary.ModifyEmpCode = empCode;
            dictionary.ModifyEmpName = empName;
            dictionary.ModifyTime = DateTime.Now;

            _context.T_SYS_Dictionary.Update(dictionary);
            return await _context.SaveChangesAsync() > 0;
        }

        // 删除数据字典（逻辑删除）
        public async Task<bool> DeleteDictionaryAsync(string dictId)
        {
            var dictionary = await _context.T_SYS_Dictionary
                .FirstOrDefaultAsync(d => d.DictId == dictId && !d.IsDeleted);

            if (dictionary == null)
            {
                return false;
            }

           var (empCode, empName) = _userContextService.GetCurrentUser();

            dictionary.IsDeleted = true;
            dictionary.ModifyEmpCode = empCode;
            dictionary.ModifyEmpName = empName;
            dictionary.ModifyTime = DateTime.Now;

            _context.T_SYS_Dictionary.Update(dictionary);
            return await _context.SaveChangesAsync() > 0;
        }

        // 获取字典详情
        public async Task<T_SYS_DictionaryModel?> GetDictionaryByIdAsync(string dictId)
        {
            return await _context.T_SYS_Dictionary
                .FirstOrDefaultAsync(d => d.DictId == dictId && !d.IsDeleted);
        }

        // 根据条件查询字典（分页）
        public async Task<(List<T_SYS_DictionaryModel> Items, int TotalCount)> QueryDictionariesAsync(DictionaryQueryDTO queryDTO)
        {
            IQueryable<T_SYS_DictionaryModel> query = _context.T_SYS_Dictionary
                .Where(d => !d.IsDeleted);

            // 应用筛选条件
            if (!string.IsNullOrEmpty(queryDTO.DictCode))
            {
                query = query.Where(d => d.DictCode.Contains(queryDTO.DictCode));
            }

            if (!string.IsNullOrEmpty(queryDTO.DictName))
            {
                query = query.Where(d => d.DictName.Contains(queryDTO.DictName));
            }

            if (!string.IsNullOrEmpty(queryDTO.DictType))
            {
                query = query.Where(d => d.DictType == queryDTO.DictType);
            }

            if (!string.IsNullOrEmpty(queryDTO.ParentId))
            {
                query = query.Where(d => d.ParentId == queryDTO.ParentId);
            }

            if (queryDTO.Status.HasValue)
            {
                query = query.Where(d => d.Status == queryDTO.Status.Value);
            }

            // 获取总数
            int totalCount = await query.CountAsync();

            // 应用分页
            var items = await query
                .OrderBy(d => d.DictType)
                .ThenBy(d => d.DictLevel)
                .ThenBy(d => d.DictOrder)
                .Skip((queryDTO.PageIndex - 1) * queryDTO.PageSize)
                .Take(queryDTO.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // 根据字典类型获取树形结构
        public async Task<List<DictionaryTreeNodeDTO>> GetDictionaryTreeByTypeAsync(string dictType)
        {
            var allDictionaries = await _context.T_SYS_Dictionary
                .Where(d => d.DictType == dictType && !d.IsDeleted)
                .OrderBy(d => d.DictOrder)
                .ToListAsync();

            // 获取根节点（ParentId为null或空字符串的节点）
            var rootNodes = allDictionaries
                .Where(d => string.IsNullOrEmpty(d.ParentId))
                .ToList();

            // 构建树形结构
            var result = new List<DictionaryTreeNodeDTO>();
            foreach (var root in rootNodes)
            {
                var treeNode = ConvertToTreeNode(root);
                BuildTree(treeNode, allDictionaries);
                result.Add(treeNode);
            }

            return result;
        }

        // 获取指定节点的所有子节点（包含子节点的子节点）
        public async Task<List<T_SYS_DictionaryModel>> GetAllChildrenAsync(string parentId)
        {
            var result = new List<T_SYS_DictionaryModel>();
            
            // 获取直接子节点
            var directChildren = await _context.T_SYS_Dictionary
                .Where(d => d.ParentId == parentId && !d.IsDeleted)
                .OrderBy(d => d.DictOrder)
                .ToListAsync();
            
            result.AddRange(directChildren);
            
            // 递归获取所有子节点
            foreach (var child in directChildren)
            {
                var childrenOfChild = await GetAllChildrenAsync(child.DictId);
                result.AddRange(childrenOfChild);
            }
            
            return result;
        }

        // 批量删除（逻辑删除）
        public async Task<bool> BatchDeleteAsync(List<string> dictIds)
        {
            if (dictIds == null || !dictIds.Any())
            {
                return false;
            }

          var (empCode, empName) = _userContextService.GetCurrentUser();
            
            var dictionaries = await _context.T_SYS_Dictionary
                .Where(d => dictIds.Contains(d.DictId) && !d.IsDeleted)
                .ToListAsync();
            
            foreach (var dictionary in dictionaries)
            {
                dictionary.IsDeleted = true;
                dictionary.ModifyEmpCode = empCode;
                dictionary.ModifyEmpName = empName;
                dictionary.ModifyTime = DateTime.Now;
            }
            
            _context.T_SYS_Dictionary.UpdateRange(dictionaries);
            return await _context.SaveChangesAsync() > 0;
        }

        // 辅助方法：将字典模型转换为树节点DTO
        private DictionaryTreeNodeDTO ConvertToTreeNode(T_SYS_DictionaryModel dictionary)
        {
            return new DictionaryTreeNodeDTO
            {
                DictId = dictionary.DictId,
                DictCode = dictionary.DictCode,
                DictName = dictionary.DictName,
                DictValue = dictionary.DictValue,
                ParentId = dictionary.ParentId,
                DictLevel = dictionary.DictLevel,
                DictOrder = dictionary.DictOrder,
                DictType = dictionary.DictType,
                DictDesc = dictionary.DictDesc,
                Status = dictionary.Status,
                CreateTime = dictionary.CreateTime,
                ModifyTime = dictionary.ModifyTime,
                Children = new List<DictionaryTreeNodeDTO>()
            };
        }

        // 辅助方法：构建树形结构
        private void BuildTree(DictionaryTreeNodeDTO parent, List<T_SYS_DictionaryModel> allDictionaries)
        {
            var children = allDictionaries
                .Where(d => d.ParentId == parent.DictId)
                .OrderBy(d => d.DictOrder)
                .ToList();

            foreach (var child in children)
            {
                var childNode = ConvertToTreeNode(child);
                parent.Children.Add(childNode);
                BuildTree(childNode, allDictionaries);
            }
        }
    }
} 