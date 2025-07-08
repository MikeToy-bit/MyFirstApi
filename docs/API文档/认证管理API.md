# 认证管理 API 文档

## 概述

本文档介绍了认证管理系统的所有 API 接口，包括用户登录、令牌刷新、令牌验证和用户登出等功能。

## API 接口列表

### 1. 用户登录

-   **接口地址**: `POST /api/auth/login`
-   **描述**: 用户登录获取访问令牌
-   **认证要求**: 无需认证
-   **请求参数**:

```json
{
    "empCode": "EMP001",
    "password": "123456"
}
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "tokenType": "Bearer",
        "expiresIn": 3600,
        "userInfo": {
            "empCode": "EMP001",
            "empName": "张三",
            "orgName": "技术部"
        }
    },
    "success": true
}
```

### 2. 刷新令牌

-   **接口地址**: `POST /api/auth/refresh`
-   **描述**: 使用刷新令牌获取新的访问令牌
-   **认证要求**: 无需认证
-   **请求参数**:

```json
"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "tokenType": "Bearer",
        "expiresIn": 3600
    },
    "success": true
}
```

### 3. 验证令牌

-   **接口地址**: `GET /api/auth/validate`
-   **描述**: 验证当前访问令牌是否有效
-   **认证要求**: 需要 Bearer Token
-   **请求头**:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "isValid": true
    },
    "success": true
}
```

### 4. 用户登出

-   **接口地址**: `POST /api/auth/logout`
-   **描述**: 用户登出，使当前令牌失效
-   **认证要求**: 需要 Bearer Token
-   **请求头**:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "message": "登出成功"
    },
    "success": true
}
```

## 安全特性

### JWT 令牌机制

-   **访问令牌**: 短期有效，用于 API 访问认证
-   **刷新令牌**: 长期有效，用于获取新的访问令牌
-   **自动过期**: 令牌具有有效期，过期后需要重新获取

### 安全措施

-   **令牌黑名单**: 登出时将令牌加入黑名单，防止令牌被重复使用
-   **密码验证**: 支持密码哈希验证（当前版本已注释）
-   **HTTPS 传输**: 建议在生产环境中使用 HTTPS 传输

## 错误处理

### 常见错误码

-   **400**: 用户不存在或密码错误
-   **401**: 令牌无效或已过期
-   **403**: 无权限访问

### 错误响应示例

```json
{
    "statusCode": 400,
    "message": "用户不存在",
    "data": null,
    "success": false
}
```

## 使用流程

### 1. 用户认证流程

```
1. 用户提交登录信息 → POST /api/auth/login
2. 系统验证用户信息
3. 返回访问令牌和刷新令牌
4. 客户端存储令牌
```

### 2. API 调用流程

```
1. 在请求头中携带访问令牌
2. 服务器验证令牌有效性
3. 返回API响应
```

### 3. 令牌刷新流程

```
1. 访问令牌过期
2. 使用刷新令牌 → POST /api/auth/refresh
3. 获取新的访问令牌
4. 继续API调用
```

## 注意事项

1. **令牌存储**: 建议将令牌存储在安全的地方，避免 XSS 攻击
2. **令牌传输**: 所有令牌相关的请求都应通过 HTTPS 传输
3. **定期刷新**: 建议在访问令牌过期前主动刷新
4. **安全登出**: 用户登出时务必调用登出接口使令牌失效
5. **错误处理**: 客户端应妥善处理认证失败的情况
