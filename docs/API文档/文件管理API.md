# 文件管理 API 文档

## 概述

本文档介绍了文件管理系统的所有 API 接口，包括文件上传、下载、查询和删除等功能。

## API 接口列表

### 1. 获取所有文件

-   **接口地址**: `GET /api/files`
-   **描述**: 获取所有文件列表
-   **认证要求**: 需要 Bearer Token
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": [
        {
            "fileId": "f123456789",
            "fileName": "document.pdf",
            "fileExtension": ".pdf",
            "fileSize": 1024000,
            "contentType": "application/pdf",
            "moduleName": "用户管理",
            "fileOperationName": "用户资料",
            "fileUrl": "/storage/202412/f123456789.pdf",
            "createTime": "2024-01-01T10:00:00",
            "createEmpName": "张三"
        }
    ],
    "success": true
}
```

### 2. 根据 ID 获取文件信息

-   **接口地址**: `GET /api/files/{fileId}`
-   **描述**: 根据文件 ID 获取文件详细信息
-   **认证要求**: 无需认证
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "fileId": "f123456789",
        "fileName": "document.pdf",
        "fileExtension": ".pdf",
        "fileSize": 1024000,
        "contentType": "application/pdf",
        "moduleName": "用户管理",
        "fileOperationName": "用户资料",
        "fileUrl": "/storage/202412/f123456789.pdf",
        "fileStatus": 1,
        "createTime": "2024-01-01T10:00:00",
        "createEmpName": "张三"
    },
    "success": true
}
```

### 3. 上传文件

-   **接口地址**: `POST /api/files/upload`
-   **描述**: 上传文件到服务器
-   **认证要求**: 需要 Bearer Token
-   **请求参数**: (multipart/form-data)

```
File: 文件对象
ModuleName: 模块名称（如：用户管理）
OperationName: 操作名称（如：用户资料）
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "fileId": "f123456789",
        "fileName": "document.pdf",
        "fileUrl": "/storage/202412/f123456789.pdf",
        "fileExtension": ".pdf",
        "fileSize": 1024000,
        "contentType": "application/pdf"
    },
    "success": true
}
```

### 4. 上传图片（带压缩）

-   **接口地址**: `POST /api/files/upload/image`
-   **描述**: 上传图片文件，支持自动压缩
-   **认证要求**: 需要 Bearer Token
-   **请求参数**: (multipart/form-data)

```
File: 图片文件对象
ModuleName: 模块名称
OperationName: 操作名称
CompressImage: 是否压缩（boolean）
MaxWidth: 最大宽度（可选）
MaxHeight: 最大高度（可选）
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "fileId": "f123456789",
        "fileName": "image.jpg",
        "fileUrl": "/storage/202412/f123456789.jpg",
        "fileExtension": ".jpg",
        "fileSize": 512000,
        "contentType": "image/jpeg"
    },
    "success": true
}
```

### 5. 下载文件

-   **接口地址**: `GET /api/files/download/{fileId}`
-   **描述**: 根据文件 ID 下载文件
-   **认证要求**: 无需认证
-   **响应**: 文件流（直接下载）

### 6. 删除文件

-   **接口地址**: `DELETE /api/files/{fileId}`
-   **描述**: 删除指定的文件（逻辑删除）
-   **认证要求**: 需要 Bearer Token
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": true,
    "success": true
}
```

## 文件存储规则

### 存储结构

```
storage/
├── 202412/           # 按年月分类
│   ├── f123456789.pdf
│   ├── f987654321.jpg
│   └── ...
└── 202501/
    └── ...
```

### 文件命名规则

-   **文件 ID**: 32 位 GUID（不含连字符）
-   **存储路径**: `/storage/{YYYYMM}/{fileId}{extension}`
-   **原始文件名**: 保存在数据库中，下载时使用

### 支持的文件类型

-   **文档类**: PDF, DOC, DOCX, XLS, XLSX, PPT, PPTX, TXT
-   **图片类**: JPG, JPEG, PNG, GIF, BMP, WEBP
-   **压缩类**: ZIP, RAR, 7Z
-   **其他类**: 根据业务需求配置

## 图片处理功能

### 自动压缩

-   **智能压缩**: 根据图片大小自动选择压缩级别
-   **尺寸限制**: 支持设置最大宽度和高度
-   **质量保证**: 在压缩文件大小的同时保证图片质量

### 压缩参数

```json
{
    "compressImage": true,
    "maxWidth": 1920,
    "maxHeight": 1080,
    "quality": 85
}
```

## 安全特性

### 文件验证

-   **文件类型检查**: 验证文件扩展名和 MIME 类型
-   **文件大小限制**: 防止上传过大文件影响系统性能
-   **恶意文件检测**: 基本的文件内容安全检查

### 访问控制

-   **用户认证**: 上传和删除操作需要用户认证
-   **权限控制**: 基于用户角色的文件访问权限
-   **操作审计**: 记录文件操作的用户和时间

## 错误处理

### 常见错误码

-   **400**: 文件为空、文件类型不支持、文件过大
-   **401**: 用户未认证
-   **404**: 文件不存在
-   **500**: 服务器存储错误

### 错误响应示例

```json
{
    "statusCode": 400,
    "message": "上传的文件不是图片",
    "data": null,
    "success": false
}
```

## 使用示例

### JavaScript 上传示例

```javascript
const formData = new FormData();
formData.append("File", fileInput.files[0]);
formData.append("ModuleName", "用户管理");
formData.append("OperationName", "用户头像");

fetch("/api/files/upload/image", {
    method: "POST",
    headers: {
        Authorization: "Bearer " + token,
    },
    body: formData,
})
    .then((response) => response.json())
    .then((data) => {
        console.log("上传成功:", data);
    });
```

### 下载链接生成

```javascript
const downloadUrl = `/api/files/download/${fileId}`;
// 可以直接用作<a>标签的href或img标签的src
```

## 注意事项

1. **文件大小**: 建议单个文件不超过 100MB
2. **存储空间**: 定期清理无用文件，避免存储空间不足
3. **安全性**: 不要上传包含敏感信息的文件
4. **备份策略**: 重要文件建议定期备份
5. **CDN 加速**: 生产环境建议配置 CDN 加速文件访问
6. **清理机制**: 系统会定期清理已删除的物理文件
