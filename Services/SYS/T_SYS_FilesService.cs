using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyFirstApi.Data;
using MyFirstApi.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MyFirstApi.Services
{
    public class T_SYS_FilesService : IT_SYS_FilesService
    {
        private readonly TestDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly string _fileStoragePath;
        
        public T_SYS_FilesService(
            TestDbContext context, 
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
            
            // 从配置中获取文件存储路径，如果不存在则使用默认路径
            _fileStoragePath = _configuration["FileStorage:Path"] ?? Path.Combine(_environment.ContentRootPath, "FileStorage");
            
            // 确保文件存储目录存在
            if (!Directory.Exists(_fileStoragePath))
            {
                Directory.CreateDirectory(_fileStoragePath);
            }
        }
        
        // 获取所有文件
        public async Task<List<T_SYS_FilesModel>> GetAllFiles()
        {
            return await _context.T_SYS_Files
                .Where(f => f.IsDeleted != true)
                .ToListAsync();
        }
        
        // 根据ID获取文件信息
        public async Task<T_SYS_FilesModel?> GetFileById(string fileId)
        {
            return await _context.T_SYS_Files
                .Where(f => f.FileId == fileId && f.IsDeleted != true)
                .FirstOrDefaultAsync();
        }
        
        // 上传文件
        public async Task<FileResponseModel> UploadFile(FileUploadModel model, string empCode, string empName)
        {
            // 生成文件ID
            string fileId = Guid.NewGuid().ToString("N");
            
            // 获取文件信息
            string fileName = model.File.FileName;
            string fileExtension = Path.GetExtension(fileName);
            long fileSize = model.File.Length;
            
            // 创建存储目录（按年月分类）
            string yearMonth = DateTime.Now.ToString("yyyyMM");
            string storageDirectory = Path.Combine(_fileStoragePath, yearMonth);
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
            
            // 生成文件存储路径
            string storagePath = Path.Combine(storageDirectory, $"{fileId}{fileExtension}");
            
            // 保存文件
            using (var fileStream = new FileStream(storagePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileStream);
            }
            
            // // 生成访问URL
            // string fileUrl = $"/api/files/upLoad/{fileId}";
            
            // 创建文件记录
            var fileRecord = new T_SYS_FilesModel
            {
                FileId = fileId,
                FileName = fileName,
                FileExtension = fileExtension,
                FileSize = fileSize,
                ModuleName = model.ModuleName,
                FileOperationName = model.OperationName,
                FileUrl = storagePath,
                ContentType = model.File.ContentType,
                StorageProvider = "local",
                FileStatus = 1, // 1表示有效
                CreateEmpCode = empCode,
                CreateEmpName = empName,
                CreateTime = DateTime.Now,
                ModifyEmpCode = empCode,
                ModifyEmpName = empName,
                ModifyTime = DateTime.Now,
                IsDeleted = false
            };
            
            // 保存到数据库
            _context.T_SYS_Files.Add(fileRecord);
            await _context.SaveChangesAsync();
            
            // 返回文件信息
            return new FileResponseModel
            {
                FileId = fileId,
                FileName = fileName,
                FileUrl = storagePath,
                FileExtension = fileExtension,
                FileSize = fileSize,
                ContentType = model.File.ContentType
            };
        }
        
        // 上传图片（含压缩功能）
        public async Task<FileResponseModel> UploadImage(FileUploadModel model, string empCode, string empName)
        {
            // 检查文件是否为图片
            if (!model.File.ContentType.StartsWith("image/"))
            {
                throw new ArgumentException("上传的文件不是图片");
            }
            
            // 生成文件ID
            string fileId = Guid.NewGuid().ToString("N");
            
            // 获取文件信息
            string fileName = model.File.FileName;
            string fileExtension = Path.GetExtension(fileName).ToLower();
            
            // 创建存储目录（按年月分类）
            string yearMonth = DateTime.Now.ToString("yyyyMM");
            string storageDirectory = Path.Combine(_fileStoragePath, yearMonth);
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
            
            // 生成文件存储路径
            string storagePath = Path.Combine(storageDirectory, $"{fileId}{fileExtension}");
            
            long fileSize;
            
            // 如果需要压缩图片
            if (model.CompressImage && (model.MaxWidth.HasValue || model.MaxHeight.HasValue))
            {
                using (var image = await Image.LoadAsync(model.File.OpenReadStream()))
                {
                    int maxWidth = model.MaxWidth ?? image.Width;
                    int maxHeight = model.MaxHeight ?? image.Height;
                    
                    // 调整大小，保持比例
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(maxWidth, maxHeight),
                        Mode = ResizeMode.Max
                    }));
                    
                    // 保存压缩后的图片
                    await image.SaveAsync(storagePath);
                }
                
                // 获取压缩后的文件大小
                FileInfo fileInfo = new FileInfo(storagePath);
                fileSize = fileInfo.Length;
            }
            else
            {
                // 直接保存原图
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileStream);
                }
                fileSize = model.File.Length;
            }
            
            // // 生成访问URL
            // string fileUrl = $"/api/files/download/{fileId}";
            
            // 创建文件记录
            var fileRecord = new T_SYS_FilesModel
            {
                FileId = fileId,
                FileName = fileName,
                FileExtension = fileExtension,
                FileSize = fileSize,
                ModuleName = model.ModuleName,
                FileOperationName = model.OperationName,
                FileUrl = storagePath,
                ContentType = model.File.ContentType,
                StorageProvider = "local",
                FileStatus = 1, // 1表示有效
                CreateEmpCode = empCode,
                CreateEmpName = empName,
                CreateTime = DateTime.Now,
                ModifyEmpCode = empCode,
                ModifyEmpName = empName,
                ModifyTime = DateTime.Now,
                IsDeleted = false
            };
            
            // 保存到数据库
            _context.T_SYS_Files.Add(fileRecord);
            await _context.SaveChangesAsync();
            
            // 返回文件信息
            return new FileResponseModel
            {
                FileId = fileId,
                FileName = fileName,
                FileUrl = storagePath,
                FileExtension = fileExtension,
                FileSize = fileSize,
                ContentType = model.File.ContentType
            };
        }
        
        // 获取文件物理路径
        public async Task<string> GetFilePhysicalPath(string fileId)
        {
            var fileInfo = await GetFileById(fileId);
            if (fileInfo == null)
            {
                throw new FileNotFoundException($"未找到ID为 {fileId} 的文件");
            }
            
            string yearMonth = fileInfo.CreateTime?.ToString("yyyyMM") ?? DateTime.Now.ToString("yyyyMM");
            return Path.Combine(_fileStoragePath, yearMonth, $"{fileId}{fileInfo.FileExtension}");
        }
        
        // 获取文件流
        public async Task<(Stream FileStream, string ContentType, string FileName)> GetFileStream(string fileId)
        {
            var fileInfo = await GetFileById(fileId);
            if (fileInfo == null)
            {
                throw new FileNotFoundException($"未找到ID为 {fileId} 的文件");
            }
            // 获取文件物理路径 
            string filePath = await GetFilePhysicalPath(fileId);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"文件 {filePath} 不存在");
            }
            
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return (fileStream, fileInfo.ContentType, fileInfo.FileName ?? "unknown");
        }
        
        // 删除文件
        public async Task<bool> DeleteFile(string fileId, string empCode, string empName)
        {
            var fileInfo = await GetFileById(fileId);
            if (fileInfo == null)
            {
                return false;
            }
            
            // 逻辑删除
            fileInfo.IsDeleted = true;
            fileInfo.FileStatus = 0; //被删除
            fileInfo.ModifyEmpCode = empCode;
            fileInfo.ModifyEmpName = empName;
            fileInfo.ModifyTime = DateTime.Now;
            
            // 尝试物理删除文件(可选)
            try
            {
                // string filePath = await GetFilePhysicalPath(fileId);
                // if (File.Exists(filePath))
                // {
                //     File.Delete(filePath);
                // }
            }
            catch
            {
                // 物理删除失败不影响逻辑删除
            }
            
            _context.T_SYS_Files.Update(fileInfo);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
} 