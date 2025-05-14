using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MyFirstApi.Models
{
    public class T_SYS_FilesModel
    {
        [Key]
        public required string FileId { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public long? FileSize { get; set; }
        public string? ModuleName { get; set; }
        public string? FileOperationName { get; set; }
        public string? FileUrl { get; set; }
        public required string ContentType { get; set; }
        public required string StorageProvider { get; set; }
        public byte? FileStatus { get; set; }
        public string? CreateEmpCode { get; set; }
        public string? CreateEmpName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? ModifyEmpCode { get; set; }
        public string? ModifyEmpName { get; set; }
        public DateTime? ModifyTime { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class FileUploadModel
    {
        public IFormFile File { get; set; } = null!;
        public string? ModuleName { get; set; }
        public string? OperationName { get; set; }
        public bool CompressImage { get; set; } = false;
        public int? MaxWidth { get; set; }
        public int? MaxHeight { get; set; }
        public int? Quality { get; set; } = 80;
    }

    public class FileResponseModel
    {
        public string FileId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
    }
} 