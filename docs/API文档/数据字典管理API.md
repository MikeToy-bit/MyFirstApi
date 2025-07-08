# 数据字典管理 API 文档

## 概述

本文档介绍了数据字典管理系统的所有 API 接口，包括字典的创建、更新、删除、查询和树形结构获取等功能。

## API 接口列表

### 1. 创建数据字典

-   **接口地址**: `POST /api/dictionary`
-   **描述**: 创建新的数据字典项
-   **认证要求**: 需要 Bearer Token
-   **请求参数**:

```json
{
    "dictCode": "GENDER",
    "dictName": "性别",
    "dictValue": "male",
    "parentId": null,
    "dictLevel": 1,
    "dictOrder": 1,
    "dictType": "SYSTEM",
    "dictDesc": "用户性别选项",
    "status": 1
}
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "dictId": "d123456789",
        "dictCode": "GENDER",
        "dictName": "性别",
        "dictValue": "male",
        "createTime": "2024-01-01T10:00:00"
    },
    "success": true
}
```

### 2. 更新数据字典

-   **接口地址**: `PUT /api/dictionary`
-   **描述**: 更新现有数据字典项
-   **认证要求**: 需要 Bearer Token
-   **请求参数**:

```json
{
    "dictId": "d123456789",
    "dictCode": "GENDER",
    "dictName": "性别选项",
    "dictValue": "male",
    "parentId": null,
    "dictLevel": 1,
    "dictOrder": 1,
    "dictType": "SYSTEM",
    "dictDesc": "用户性别选项（更新）",
    "status": 1
}
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": true,
    "success": true
}
```

### 3. 删除数据字典

-   **接口地址**: `DELETE /api/dictionary/{dictId}`
-   **描述**: 删除指定的数据字典项（逻辑删除）
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

### 4. 获取数据字典详情

-   **接口地址**: `GET /api/dictionary/{dictId}`
-   **描述**: 根据字典 ID 获取详细信息
-   **认证要求**: 需要 Bearer Token
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": {
        "dictId": "d123456789",
        "dictCode": "GENDER",
        "dictName": "性别",
        "dictValue": "male",
        "parentId": null,
        "dictLevel": 1,
        "dictOrder": 1,
        "dictType": "SYSTEM",
        "dictDesc": "用户性别选项",
        "status": 1,
        "createTime": "2024-01-01T10:00:00",
        "createEmpName": "张三"
    },
    "success": true
}
```

### 5. 分页查询数据字典

-   **接口地址**: `POST /api/dictionary/query`
-   **描述**: 支持多条件筛选的分页查询
-   **认证要求**: 需要 Bearer Token
-   **请求参数**:

```json
{
    "dictCode": "GENDER",
    "dictName": "性别",
    "dictType": "SYSTEM",
    "parentId": null,
    "status": 1,
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
                "dictId": "d123456789",
                "dictCode": "GENDER",
                "dictName": "性别",
                "dictValue": "male",
                "dictType": "SYSTEM",
                "status": 1,
                "createTime": "2024-01-01T10:00:00"
            }
        ],
        "totalCount": 50,
        "pageIndex": 1,
        "pageSize": 10,
        "totalPages": 5
    },
    "success": true
}
```

### 6. 获取树形结构数据

-   **接口地址**: `GET /api/dictionary/tree/{dictType}`
-   **描述**: 根据字典类型获取树形结构数据
-   **认证要求**: 需要 Bearer Token
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": [
        {
            "dictId": "d123456789",
            "dictCode": "REGION",
            "dictName": "地区",
            "dictValue": "china",
            "dictLevel": 1,
            "dictOrder": 1,
            "children": [
                {
                    "dictId": "d987654321",
                    "dictCode": "PROVINCE",
                    "dictName": "省份",
                    "dictValue": "beijing",
                    "dictLevel": 2,
                    "dictOrder": 1,
                    "children": []
                }
            ]
        }
    ],
    "success": true
}
```

### 7. 获取指定节点的所有子节点

-   **接口地址**: `GET /api/dictionary/children/{parentId}`
-   **描述**: 获取指定父节点下的所有子节点（包含子节点的子节点）
-   **认证要求**: 需要 Bearer Token
-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": [
        {
            "dictId": "d987654321",
            "dictCode": "PROVINCE",
            "dictName": "省份",
            "parentId": "d123456789",
            "dictLevel": 2
        },
        {
            "dictId": "d111111111",
            "dictCode": "CITY",
            "dictName": "城市",
            "parentId": "d987654321",
            "dictLevel": 3
        }
    ],
    "success": true
}
```

### 8. 批量删除数据字典

-   **接口地址**: `POST /api/dictionary/batch-delete`
-   **描述**: 批量删除多个数据字典项
-   **认证要求**: 需要 Bearer Token
-   **请求参数**:

```json
["d123456789", "d987654321", "d111111111"]
```

-   **响应示例**:

```json
{
    "statusCode": 200,
    "message": "操作成功",
    "data": true,
    "success": true
}
```

## 数据字典结构

### 基本字段说明

-   **dictId**: 字典唯一标识符
-   **dictCode**: 字典编码（业务编码）
-   **dictName**: 字典名称（显示名称）
-   **dictValue**: 字典值（实际值）
-   **parentId**: 父节点 ID（用于构建树形结构）
-   **dictLevel**: 层级（从 1 开始）
-   **dictOrder**: 排序序号
-   **dictType**: 字典类型（用于分类）
-   **dictDesc**: 字典描述
-   **status**: 状态（1:启用, 0:禁用）

### 层级结构示例

```
系统配置 (SYSTEM)
├── 用户状态 (USER_STATUS)
│   ├── 正常 (NORMAL)
│   ├── 禁用 (DISABLED)
│   └── 锁定 (LOCKED)
├── 性别 (GENDER)
│   ├── 男 (MALE)
│   └── 女 (FEMALE)
└── 地区 (REGION)
    ├── 中国 (CHINA)
    │   ├── 北京 (BEIJING)
    │   └── 上海 (SHANGHAI)
    └── 美国 (USA)
```

## 业务规则

### 编码规则

-   **dictType**: 全大写，下划线分隔，如：USER_STATUS, SYSTEM_CONFIG
-   **dictCode**: 全大写，下划线分隔，如：NORMAL, DISABLED
-   **dictValue**: 小写，下划线分隔，如：normal, disabled

### 层级规则

-   **根节点**: parentId 为 null 或空字符串
-   **子节点**: parentId 为父节点的 dictId
-   **层级限制**: 建议不超过 5 层
-   **排序规则**: 同级节点按 dictOrder 升序排列

### 状态管理

-   **启用状态**: status = 1，可正常使用
-   **禁用状态**: status = 0，不显示在前端选择项中
-   **删除状态**: 逻辑删除，isDeleted = true

## 使用场景

### 1. 下拉选择框

```javascript
// 获取性别选项
fetch("/api/dictionary/tree/GENDER", {
    headers: { Authorization: "Bearer " + token },
})
    .then((response) => response.json())
    .then((data) => {
        const options = data.data.map((item) => ({
            value: item.dictValue,
            label: item.dictName,
        }));
    });
```

### 2. 级联选择器

```javascript
// 获取地区树形数据
fetch("/api/dictionary/tree/REGION", {
    headers: { Authorization: "Bearer " + token },
})
    .then((response) => response.json())
    .then((data) => {
        const treeData = data.data; // 直接用于级联选择器
    });
```

### 3. 动态表单

```javascript
// 根据字典类型动态生成表单项
const formConfig = {
    gender: { dictType: "GENDER", type: "radio" },
    region: { dictType: "REGION", type: "cascader" },
    status: { dictType: "USER_STATUS", type: "select" },
};
```

## 缓存策略

### 前端缓存

-   **本地存储**: 将常用字典数据存储在 localStorage 中
-   **定期更新**: 设置缓存过期时间，定期从服务器更新
-   **版本控制**: 通过版本号判断是否需要更新缓存

### 服务端缓存

-   **内存缓存**: 系统启动时加载常用字典到内存
-   **Redis 缓存**: 使用 Redis 缓存字典数据，提高查询性能
-   **缓存更新**: 字典数据变更时及时更新缓存

## 错误处理

### 常见错误码

-   **400**: 请求参数错误、字典编码重复
-   **401**: 用户未认证
-   **404**: 字典项不存在
-   **500**: 服务器内部错误

### 错误响应示例

```json
{
    "statusCode": 400,
    "message": "字典编码已存在",
    "data": null,
    "success": false
}
```

## 注意事项

1. **编码唯一性**: 同一 dictType 下的 dictCode 必须唯一
2. **层级关系**: 删除父节点时需要处理子节点
3. **状态继承**: 父节点禁用时，子节点也应禁用
4. **排序维护**: 新增节点时需要合理设置 dictOrder
5. **缓存更新**: 字典数据变更后及时更新相关缓存
6. **权限控制**: 字典管理操作需要相应的管理权限
