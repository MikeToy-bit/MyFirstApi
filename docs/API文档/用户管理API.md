# 用户管理 API 文档

## 概述

本文档介绍了用户管理系统的所有 API 接口，包括用户的查询、创建、更新、删除等功能。

## API 接口列表

### 1. 获取用户信息（简单列表）

-   **接口地址**: `GET /api/user`
-   **描述**: 获取所有未删除的用户列表
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": [
        {
            "empCode": "EMP001",
            "empName": "张三",
            "orgCode": "ORG001",
            "orgName": "技术部"
        }
    ],
    "success": true
}
```

### 2. 分页查询用户信息

-   **接口地址**: `POST /api/user/query`
-   **描述**: 支持多条件筛选的分页查询
-   **请求参数**:

```json
{
    "empCode": "EMP",
    "empName": "张",
    "orgCode": "ORG001",
    "orgName": "技术部",
    "postCode": "POST001",
    "postName": "开发工程师",
    "sex": true,
    "phoneNumber": "138",
    "idCard": "110",
    "startCreateTime": "2024-01-01T00:00:00",
    "endCreateTime": "2024-12-31T23:59:59",
    "pageIndex": 1,
    "pageSize": 10
}
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "items": [
            {
                "empCode": "EMP001",
                "empName": "张三",
                "orgCode": "ORG001",
                "orgName": "技术部",
                "postCode": "POST001",
                "postName": "开发工程师",
                "sex": true,
                "phoneNumber": "13812345678",
                "createTime": "2024-01-01T10:00:00"
            }
        ],
        "totalCount": 100,
        "pageIndex": 1,
        "pageSize": 10,
        "totalPages": 10
    },
    "success": true
}
```

### 3. 根据员工编号获取用户详情

-   **接口地址**: `GET /api/user/{empCode}`
-   **描述**: 获取指定员工的详细信息
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "empCode": "EMP001",
        "empName": "张三",
        "orgCode": "ORG001",
        "orgName": "技术部",
        "postCode": "POST001",
        "postName": "开发工程师",
        "sex": true,
        "idType": "身份证",
        "idCard": "110101199001011234",
        "birthday": "1990-01-01T00:00:00",
        "phoneNumber": "13812345678",
        "createTime": "2024-01-01T10:00:00"
    },
    "success": true
}
```

### 4. 创建用户

-   **接口地址**: `POST /api/user`
-   **描述**: 创建新用户
-   **请求参数**:

```json
{
    "empCode": "EMP002",
    "empName": "李四",
    "password": "123456",
    "orgCode": "ORG001",
    "orgName": "技术部",
    "postCode": "POST001",
    "postName": "开发工程师",
    "sex": false,
    "idType": "身份证",
    "idCard": "110101199002021234",
    "birthday": "1990-02-02T00:00:00",
    "phoneNumber": "13812345679"
}
```

### 5. 更新用户信息

-   **接口地址**: `PUT /api/user`
-   **描述**: 更新现有用户信息
-   **请求参数**:

```json
{
    "empCode": "EMP002",
    "empName": "李四",
    "password": "新密码",
    "orgCode": "ORG002",
    "orgName": "市场部",
    "postCode": "POST002",
    "postName": "市场专员",
    "sex": false,
    "phoneNumber": "13812345680"
}
```

### 6. 删除用户（逻辑删除）

-   **接口地址**: `DELETE /api/user/{empCode}`
-   **描述**: 逻辑删除指定用户
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "用户删除成功",
    "data": true,
    "success": true
}
```

### 7. 批量删除用户

-   **接口地址**: `POST /api/user/batch-delete`
-   **描述**: 批量逻辑删除多个用户
-   **请求参数**:

```json
["EMP001", "EMP002", "EMP003"]
```

### 8. 恢复已删除的用户

-   **接口地址**: `POST /api/user/restore/{empCode}`
-   **描述**: 恢复已被逻辑删除的用户
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "用户恢复成功",
    "data": true,
    "success": true
}
```

### 9. 获取用户菜单

-   **接口地址**: `GET /api/user/getMenusByToken`
-   **描述**: 根据当前登录用户的 Token 获取菜单权限
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": [
        {
            "menuId": "MENU001",
            "menuName": "用户管理",
            "menuType": "menu",
            "menuUrl": "/user",
            "children": []
        }
    ],
    "success": true
}
```

## 技术架构

### 反射动态查询构建器

本系统采用了基于反射的动态查询构建器，具有以下优势：

#### 🚀 **核心特性**

1. **自动映射**: 基于 DTO 属性名自动匹配实体属性，无需手动编写查询条件
2. **类型安全**: 支持多种数据类型的自动识别和处理
3. **智能过滤**: 自动跳过空值、分页参数和特殊属性
4. **扩展性强**: 新增查询字段只需在 DTO 中添加属性即可
5. **服务封装**: 映射逻辑封装在服务层，提高代码可读性和维护性

#### 🛠️ **支持的查询类型**

-   **字符串查询**: 自动使用 Contains 进行模糊匹配
-   **精确匹配**: 数值、布尔值等类型的精确查询
-   **可空类型**: 正确处理可空的布尔值等类型
-   **日期范围**: 支持开始时间和结束时间的范围查询

#### 📝 **使用示例**

```csharp
// 原始方法（88行代码）
if (!string.IsNullOrEmpty(queryDTO.EmpCode))
{
    query = query.Where(u => u.EmpCode.Contains(queryDTO.EmpCode));
}
// ... 更多重复代码

// 现在的方法（简洁的service实现）
public async Task<(List<UserResponseDTO> Items, int TotalCount)> QueryUsersAsync(UserQueryDTO queryDTO)
{
    var baseQuery = _context.T_SYS_User
        .Where(u => u.IsDeleted != true)
        .ApplyFiltersFromDTO<T_SYS_UserModel, UserQueryDTO>(queryDTO)
        .WhereDateRange(queryDTO.StartCreateTime, queryDTO.EndCreateTime, u => u.CreateTime);

    var totalCount = await baseQuery.CountAsync();
    var items = await baseQuery
        .OrderByDescending(u => u.CreateTime)
        .Skip((queryDTO.PageIndex - 1) * queryDTO.PageSize)
        .Take(queryDTO.PageSize)
        .Select(u => MapToUserResponseDTO(u))
        .ToListAsync();

    return (items, totalCount);
}

// 映射方法封装在service中
private static UserResponseDTO MapToUserResponseDTO(T_SYS_UserModel user)
{
    return new UserResponseDTO
    {
        EmpCode = user.EmpCode,
        EmpName = user.EmpName,
        // ... 其他属性映射
    };
}
```

#### ⚡ **性能优势**

-   **编译时优化**: 生成的 Expression 树在运行时被编译为高效的委托
-   **SQL 优化**: Entity Framework 能够将 Expression 树转换为优化的 SQL 查询
-   **内存友好**: 避免了大量的字符串拼接和条件判断
-   **服务层映射**: 映射逻辑在服务层统一管理，便于优化和缓存

#### 🏗️ **架构优势**

-   **职责分离**: 查询逻辑、映射逻辑、业务逻辑分离明确
-   **易于测试**: 映射方法可以独立进行单元测试
-   **代码复用**: 映射逻辑在服务内部可复用
-   **维护友好**: 所有用户相关的数据处理逻辑集中在 UserService 中

## 注意事项

1. **认证授权**: 所有接口都需要用户登录认证（除了登录接口本身）
2. **权限控制**: 具体的操作权限由用户的角色和菜单权限决定
3. **数据验证**: 所有输入数据都会进行验证，确保数据的完整性和安全性
4. **逻辑删除**: 删除操作采用逻辑删除方式，数据不会真正从数据库中移除
5. **分页查询**: 支持多字段组合查询，提高查询效率
6. **密码安全**: 实际生产环境中需要对密码进行加密处理
7. **反射性能**: 动态查询构建器使用反射，在高并发场景下建议配合缓存使用

## 错误码说明

-   **200**: 操作成功
-   **400**: 请求参数错误
-   **401**: 未授权访问
-   **403**: 无权限访问
-   **404**: 资源未找到
-   **500**: 服务器内部错误
