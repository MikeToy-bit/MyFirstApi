using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyFirstApi.Data;
using MyFirstApi.Services;
using MyFirstApi.Models;


var builder = WebApplication.CreateBuilder(args);

// 配置JWT设置
builder.Services.Configure<T_SYS_JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// 添加认证服务
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<T_SYS_JwtSettings>() 
            ?? throw new InvalidOperationException("JwtSettings configuration is missing");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 配置数据库连接  
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection") + 
    ";ConnectionTimeout=120;Pooling=true;MinPoolSize=5;MaxPoolSize=100";

builder.Services.AddDbContext<TestDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOptions =>
    {
        // 配置连接池
        mysqlOptions.MinBatchSize(1);
        mysqlOptions.MaxBatchSize(100);
        mysqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        
        // 配置重试策略
        mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
            
        // 配置命令超时
        mysqlOptions.CommandTimeout(120);
    });
    
    // 启用详细错误和敏感数据日志
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
    
    // 配置连接池
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// 注册服务
builder.Services.AddScoped<T_sys_testService>();
builder.Services.AddScoped<T_SYS_userInfoService>();
builder.Services.AddScoped<IT_SYS_AuthService, T_SYS_AuthService>();
builder.Services.AddSingleton<IT_SYS_TokenBlacklistService, T_SYS_TokenBlacklistService>();
builder.Services.AddScoped<IT_SYS_RoleMenuService, T_SYS_RoleMenuService>();
builder.Services.AddScoped<IT_SYS_UserRoleService, T_SYS_UserRoleService>();
builder.Services.AddScoped<IT_SYS_FilesService, T_SYS_FilesService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IT_SYS_UserContextService, T_SYS_UserContextService>();

// 添加CORS配置
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// 配置 Kestrel 服务器只监听 HTTP
app.Urls.Add("http://localhost:7259");

// 检查是否是生产环境
app.MapGet("/check-environment", (IWebHostEnvironment env) =>
{
    return Results.Ok(new { IsProduction = env.IsProduction() });
});



// 开发环境下使用开发者异常页面
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 使用 HTTPS 重定向
//app.UseHttpsRedirection();

app.UseRouting();

// 添加认证和授权中间件
app.UseAuthentication();
app.UseAuthorization();

// 使用CORS中间件
app.UseCors("AllowAllOrigins");

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

