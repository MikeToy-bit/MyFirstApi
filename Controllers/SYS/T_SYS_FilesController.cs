using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class T_SYS_FilesController : ControllerBase
    {
        private readonly IT_SYS_FilesService _filesService;
        private readonly IT_SYS_UserContextService _userContextService;

        public T_SYS_FilesController(IT_SYS_FilesService filesService, IT_SYS_UserContextService userContextService)
        {
            _filesService = filesService;
            _userContextService = userContextService;
        }
        // 获取所有文件
        [HttpGet]
        [Authorize]
        public async Task<ApiResponse<List<T_SYS_FilesModel>>> GetAllFiles()
        {
            try
            {
                var files = await _filesService.GetAllFiles();
                return new ApiResponse<List<T_SYS_FilesModel>>(files);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<T_SYS_FilesModel>>(500, ex.Message);
            }
        }

        // 获取文件信息
        [HttpGet("{fileId}")]
        public async Task<ApiResponse<T_SYS_FilesModel>> GetFileById(string fileId)
        {
            try
            {
                var file = await _filesService.GetFileById(fileId);
                if (file == null)
                {
                    return new ApiResponse<T_SYS_FilesModel>(404, "文件不存在");
                }
                return new ApiResponse<T_SYS_FilesModel>(file);
            }
            catch (Exception ex)
            {
                return new ApiResponse<T_SYS_FilesModel>(500, ex.Message);
            }
        }

        // 上传文件
        [HttpPost("upload")]
        [Authorize]
        public async Task<ApiResponse<FileResponseModel>> UploadFile([FromForm] FileUploadModel model)
        {
            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    return new ApiResponse<FileResponseModel>(400, "未提供文件或文件为空");
                }

                // 获取当前用户信息
                var (empCode, empName) = _userContextService.GetCurrentUser();

                var result = await _filesService.UploadFile(model, empCode, empName);
                return new ApiResponse<FileResponseModel>(result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<FileResponseModel>(500, ex.Message);
            }
        }

        // 上传图片（带压缩）
        [HttpPost("upload/image")]
        [Authorize]
        public async Task<ApiResponse<FileResponseModel>> UploadImage([FromForm] FileUploadModel model)
        {
            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    return new ApiResponse<FileResponseModel>(400, "未提供图片或图片为空");
                }

                if (!model.File.ContentType.StartsWith("image/"))
                {
                    return new ApiResponse<FileResponseModel>(400, "上传的文件不是图片");
                }

                // 获取当前用户信息
                var (empCode, empName) = _userContextService.GetCurrentUser();

                var result = await _filesService.UploadImage(model, empCode, empName);
                return new ApiResponse<FileResponseModel>(result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<FileResponseModel>(500, ex.Message);
            }
        }

        // 下载文件
        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            try
            {
                var (fileStream, contentType, fileName) = await _filesService.GetFileStream(fileId);
                return File(fileStream, contentType, fileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, ex.Message));
            }
        }

        // 删除文件
        [HttpDelete("{fileId}")]
        [Authorize]
        public async Task<ApiResponse<bool>> DeleteFile(string fileId)
        {
            try
            {
                // 获取当前用户信息
                var (empCode, empName) = _userContextService.GetCurrentUser();
                
                var result = await _filesService.DeleteFile(fileId, empCode, empName);
                if (!result)
                {
                    return new ApiResponse<bool>(404, "文件不存在");
                }
                return new ApiResponse<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, ex.Message);
            }
        }
    }
} 