using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    // 认证服务接口
    public interface IT_SYS_AuthService
    {
        T_SYS_TokenResponse GenerateTokens(T_SYS_UserModel user);
        T_SYS_TokenResponse RefreshAccessToken(string refreshToken);
        bool ValidateToken(string token);
        bool VerifyPassword(string inputPassword, string storedPasswordHash);
        Task InvalidateToken(string token);
    }

    // 认证服务实现
    public class T_SYS_AuthService : IT_SYS_AuthService
    {
        // 依赖注入
        private readonly T_SYS_JwtSettings _jwtSettings;
        private readonly IT_SYS_TokenBlacklistService _blacklistService;
        private readonly IT_SYS_UserRoleService _userRoleService;
        // 构造函数
        public T_SYS_AuthService(
            IOptions<T_SYS_JwtSettings> jwtSettings,
            IT_SYS_TokenBlacklistService blacklistService,
            IT_SYS_UserRoleService userRoleService)
        {
            _jwtSettings = jwtSettings.Value;
            _blacklistService = blacklistService;
            _userRoleService = userRoleService;
        }

        public T_SYS_TokenResponse GenerateTokens(T_SYS_UserModel user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            
            return new T_SYS_TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpireMinutes),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays)
            };
        }
        // 刷新Access Token 
        public T_SYS_TokenResponse RefreshAccessToken(string refreshToken)
        {
            // 验证Refresh Token
            if (!ValidateToken(refreshToken))
            {
                throw new SecurityTokenException("无效的Refresh Token");
            }

            // 从Refresh Token中获取用户信息
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(refreshToken);
            var empCode = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(empCode))
            {
                throw new SecurityTokenException("无效的Refresh Token");
            }

            // 生成新的Access Token
            var user = new T_SYS_UserModel { EmpCode = empCode, Password = string.Empty };
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            return new T_SYS_TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpireMinutes),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays)
            };
        }

        private string GenerateAccessToken(T_SYS_UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = _userRoleService.GetRolesByEmpCode(user.EmpCode);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.EmpCode),
                new Claim("EmpCode", user.EmpCode??string.Empty),
                new Claim("EmpName", user.EmpName??string.Empty),
                new Claim("OrgCode", user.OrgCode??string.Empty),   
                new Claim("OrgName", user.OrgName??string.Empty),
                new Claim("PostCode", user.PostCode??string.Empty),
                new Claim("PostName", user.EmpName??string.Empty),
                new Claim("Sex", user.Sex.HasValue ? (user.Sex.Value ? "男" : "女") : string.Empty),
                new Claim("Birthday", user.Birthday?.ToString("yyyy-MM-dd")??string.Empty),
                new Claim("PhoneNumber", user.PhoneNumber??string.Empty),
                new Claim("roles",string.Join(",",roles))
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim("tokenType", "refresh")
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // 验证Token
        public bool ValidateToken(string token)
        {
            if (_blacklistService.IsBlacklisted(token).Result)
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // 验证密码哈希
        public bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            try
            {
                // 将输入的密码转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputPassword);
                
                // 使用SHA256计算哈希值
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);
                    string inputHash = Convert.ToBase64String(hashBytes);
                    
                    // 比较哈希值
                    return inputHash == storedPasswordHash;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task InvalidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            await _blacklistService.AddToBlacklist(token, jwtToken.ValidTo);
        }
    }
} 