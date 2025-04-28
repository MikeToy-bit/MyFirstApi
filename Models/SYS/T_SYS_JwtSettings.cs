using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models
{
    public class T_SYS_JwtSettings
    {
        [Required]
        // 密钥
        public string SecretKey { get; set; } = string.Empty;
        // 发行人
        [Required]
        public string Issuer { get; set; } = string.Empty;
        // 受众
        [Required]
        public string Audience { get; set; } = string.Empty;
        // Access Token过期时间（分钟）
        [Required]
        public int AccessTokenExpireMinutes { get; set; }
        // Refresh Token过期时间（天）
        [Required]
        public int RefreshTokenExpireDays { get; set; }
    }
} 