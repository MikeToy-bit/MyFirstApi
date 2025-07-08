# MyFirstApi - 企业级管理系统 API

## 📋 项目概述

MyFirstApi 是一个基于.NET Core 构建的现代化企业级管理系统 API，采用先进的技术架构和设计模式，为企业提供完整的用户管理、认证授权、文件管理和数据字典等核心功能。

## ✨ 核心特性

### 🚀 **技术特性**

-   **反射动态查询**: 基于反射的智能查询构建器，大幅简化查询代码
-   **JWT 认证**: 无状态的安全认证机制
-   **RESTful API**: 标准化的 API 设计规范
-   **实体框架**: Entity Framework Core ORM 数据访问
-   **异步编程**: 高性能的异步处理模式

### 🛡️ **安全特性**

-   **身份认证**: JWT 令牌认证 + 刷新机制
-   **权限控制**: 基于角色的访问控制(RBAC)
-   **数据保护**: 逻辑删除 + 操作审计
-   **文件安全**: 严格的文件类型和大小验证

### 💾 **数据管理**

-   **用户管理**: 完整的用户 CRUD 操作
-   **分页查询**: 高效的分页和搜索功能
-   **数据字典**: 灵活的配置管理系统
-   **文件存储**: 安全的文件上传和管理

## 🏗️ 架构设计

```
MyFirstApi/
├── Controllers/          # 控制器层
│   └── SYS/             # 系统模块控制器
├── Services/            # 服务层
│   └── SYS/             # 系统模块服务
├── Models/              # 数据模型层
│   └── SYS/             # 系统模块模型
├── Extensions/          # 扩展方法
├── Data/                # 数据访问层
└── docs/                # API文档
    └── API文档/          # 详细API文档
```

## 📚 文档导航

### 📖 **API 文档**

-   [📋 API 文档总览](./docs/API文档/README.md) - 完整的 API 文档指南
-   [🔐 认证管理 API](./docs/API文档/认证管理API.md) - 登录、令牌管理
-   [👥 用户管理 API](./docs/API文档/用户管理API.md) - 用户 CRUD、分页查询
-   [📁 文件管理 API](./docs/API文档/文件管理API.md) - 文件上传、下载
-   [📖 数据字典 API](./docs/API文档/数据字典管理API.md) - 字典管理

## 🚀 快速开始

### 环境要求

-   .NET 6.0 或更高版本
-   SQL Server 2019 或更高版本
-   Visual Studio 2022 或 VS Code

### 克隆项目

```bash
git clone https://github.com/your-username/MyFirstApi.git
cd MyFirstApi
```

### 配置数据库

1. 修改 `appsettings.json` 中的数据库连接字符串
2. 运行数据库迁移

```bash
dotnet ef database update
```

### 运行项目

```bash
dotnet run
```

### 访问 API

-   **Swagger 文档**: `https://localhost:5001/swagger`
-   **API 基地址**: `https://localhost:5001/api`

## 🎯 核心功能演示

### 1. 用户认证

```bash
# 登录获取Token
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"empCode":"admin","password":"123456"}'
```

### 2. 分页查询用户

```bash
# 分页查询用户（需要Token）
curl -X POST "https://localhost:5001/api/user/query" \
  -H "Authorization: Bearer {your-token}" \
  -H "Content-Type: application/json" \
  -d '{"pageIndex":1,"pageSize":10}'
```

### 3. 文件上传

```bash
# 上传文件
curl -X POST "https://localhost:5001/api/files/upload" \
  -H "Authorization: Bearer {your-token}" \
  -F "File=@/path/to/file.pdf" \
  -F "ModuleName=测试模块"
```

## 🏆 技术亮点

### 反射动态查询构建器

```csharp
// 传统方式（88行代码）
if (!string.IsNullOrEmpty(queryDTO.EmpCode))
    query = query.Where(u => u.EmpCode.Contains(queryDTO.EmpCode));
// ... 大量重复代码

// 现在的方式（6行代码）
var baseQuery = _context.T_SYS_User
    .Where(u => u.IsDeleted != true)
    .ApplyFiltersFromDTO<T_SYS_UserModel, UserQueryDTO>(queryDTO)
    .WhereDateRange(queryDTO.StartCreateTime, queryDTO.EndCreateTime, u => u.CreateTime);
```

### 智能映射系统

-   **自动类型识别**: 字符串(Contains)、布尔值(精确匹配)、日期范围
-   **空值过滤**: 自动跳过 null 值和分页参数
-   **性能优化**: Expression 树编译为高效委托

## 📊 性能表现

-   **查询效率**: 反射动态查询比传统方式减少 80%代码量
-   **响应时间**: 平均 API 响应时间 < 100ms
-   **并发处理**: 支持高并发请求处理
-   **内存优化**: 智能缓存策略，减少数据库查询

## 🛠️ 开发工具

### 推荐 IDE

-   **Visual Studio 2022**: 完整的开发体验
-   **Visual Studio Code**: 轻量级开发环境
-   **JetBrains Rider**: 跨平台 C#开发

### 调试工具

-   **Swagger UI**: API 接口测试
-   **Postman**: API 调试工具
-   **SQL Server Management Studio**: 数据库管理

## 🔧 扩展指南

### 添加新的模块

1. 在 `Controllers` 中创建新的控制器
2. 在 `Services` 中实现业务逻辑
3. 在 `Models` 中定义数据模型
4. 使用反射动态查询构建器简化查询逻辑

### 自定义查询条件

```csharp
// 扩展DynamicQueryExtensions类
public static IQueryable<T> ApplyCustomFilters<T>(this IQueryable<T> query, CustomQueryDTO dto)
{
    return query.ApplyFiltersFromDTO<T, CustomQueryDTO>(dto);
}
```

## 📈 未来规划

### 短期目标

-   [ ] 添加 Redis 缓存支持
-   [ ] 完善单元测试覆盖
-   [ ] 增加 API 版本控制
-   [ ] 优化文件存储策略

### 长期目标

-   [ ] 微服务架构升级
-   [ ] 容器化部署支持
-   [ ] 多数据库支持
-   [ ] 实时通信功能

## 👥 贡献指南

### 代码贡献

1. Fork 本项目
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 创建 Pull Request

### 问题反馈

-   通过 [Issues](https://github.com/your-username/MyFirstApi/issues) 反馈问题
-   提供详细的问题描述和复现步骤
-   建议包含相关的代码片段

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情

## 🙏 致谢

-   感谢 .NET Core 团队提供优秀的框架
-   感谢 Entity Framework Core 团队的持续改进
-   感谢所有为开源社区贡献的开发者

---

> 💡 **提示**: 如需了解更多详细信息，请查看 [API 文档](./docs/API文档/README.md)

> 📧 **联系方式**: 如有任何问题，欢迎通过 Issues 或邮件联系
