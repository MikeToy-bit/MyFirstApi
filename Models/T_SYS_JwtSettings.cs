using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models
{
    public class T_SYS_JwtSettings
    {
        [Required]
        public string SecretKey { get; set; } = string.Empty;
        
        [Required]
        public string Issuer { get; set; } = string.Empty;
        
        [Required]
        public string Audience { get; set; } = string.Empty;
        
        [Required]
        public int ExpireMinutes { get; set; }
    }
} 