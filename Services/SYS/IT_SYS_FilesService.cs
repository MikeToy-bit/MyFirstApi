using System.Collections.Generic;
using System.Threading.Tasks;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public interface IT_SYS_FilesService
    {
        // 获取所有文件
        Task<List<T_SYS_FilesModel>> GetAllFiles();
        
        // 根据ID获取文件信息
        Task<T_SYS_FilesModel?> GetFileById(string fileId);
        
        // 上传文件
        Task<FileResponseModel> UploadFile(FileUploadModel model, string empCode, string empName);
        
        // 上传图片（含压缩功能）
        Task<FileResponseModel> UploadImage(FileUploadModel model, string empCode, string empName);
        
        // 获取文件物理路径
        Task<string> GetFilePhysicalPath(string fileId);
        
        // 获取文件流
        Task<(Stream FileStream, string ContentType, string FileName)> GetFileStream(string fileId);
        
        // 删除文件
        Task<bool> DeleteFile(string fileId, string empCode, string empName);
    }
} 