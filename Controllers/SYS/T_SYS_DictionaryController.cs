using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DictionaryController : ControllerBase
    {
        private readonly IT_SYS_DictionaryService _dictionaryService;

        public DictionaryController(IT_SYS_DictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        /// <summary>
        /// 创建数据字典
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DictionaryCreateRequestDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.BadRequest());
                }

                var result = await _dictionaryService.CreateDictionaryAsync(createDTO);
                return Ok(ApiResponse<object>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 更新数据字典
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DictionaryUpdateRequestDTO updateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(ApiResponse<object>.BadRequest());
                }

                var result = await _dictionaryService.UpdateDictionaryAsync(updateDTO);
                if (!result)
                {
                    return Ok(ApiResponse<object>.NotFound());
                }

                    return Ok(ApiResponse<object>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        [HttpDelete("{dictId}")]
        public async Task<IActionResult> Delete(string dictId)
        {
            try
            {
                if (string.IsNullOrEmpty(dictId))
                {
                    return Ok(ApiResponse<object>.BadRequest());
                }

                var result = await _dictionaryService.DeleteDictionaryAsync(dictId);
                if (!result)
                {
                    return Ok(ApiResponse<object>.NotFound());
                }

                return Ok(ApiResponse<object>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 获取数据字典详情
        /// </summary>
        [HttpGet("{dictId}")]
        public async Task<IActionResult> GetById(string dictId)
        {
            try
            {
                if (string.IsNullOrEmpty(dictId))
                {
                    return Ok(ApiResponse<object>.BadRequest());
                }

                var result = await _dictionaryService.GetDictionaryByIdAsync(dictId);
                if (result == null)
                {
                    return Ok(ApiResponse<object>.NotFound());
                }

                return Ok(ApiResponse<object>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 分页查询数据字典
        /// </summary>
        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] DictionaryQueryDTO queryDTO)
        {
            try
            {
                var (items, totalCount) = await _dictionaryService.QueryDictionariesAsync(queryDTO);
                
                var result = new
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageIndex = queryDTO.PageIndex,
                    PageSize = queryDTO.PageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)queryDTO.PageSize)
                };

                return Ok(ApiResponse<object>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        [HttpGet("tree/{dictType}")]
        public async Task<IActionResult> GetTree(string dictType)
        {
            try
            {
                if (string.IsNullOrEmpty(dictType))
                {
                    return Ok(ApiResponse<object>.BadRequest());
                }

                var result = await _dictionaryService.GetDictionaryTreeByTypeAsync(dictType);

                return Ok(ApiResponse<List<DictionaryTreeNodeDTO>>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 获取指定节点的所有子节点
        /// </summary>
        [HttpGet("children/{parentId}")]
        public async Task<IActionResult> GetChildren(string parentId)
        {
            try
            {
                if (string.IsNullOrEmpty(parentId))
                {
                    return Ok(ApiResponse<object>.BadRequest());
                }

                var result = await _dictionaryService.GetAllChildrenAsync(parentId);

                return Ok(ApiResponse<object>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }

        /// <summary>
        /// 批量删除数据字典
        /// </summary>
        [HttpPost("batch-delete")]
        public async Task<IActionResult> BatchDelete([FromBody] List<string> dictIds)
        {
            try
            {
                if (dictIds == null || dictIds.Count == 0)
                {
                    return Ok(ApiResponse<object>.BadRequest());
                }

                var result = await _dictionaryService.BatchDeleteAsync(dictIds);
                if (!result)
                {
                    return Ok(ApiResponse<object>.NotFound());
                }

                return Ok(ApiResponse<object>.Ok(result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Error(ex.Message));
            }
        }
    }
} 