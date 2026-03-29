# MyNet8Api - .NET 8 Web API

一个基于 .NET 8 的 Web API 示例项目，提供用户管理和产品管理的 RESTful API 接口，可供其他应用程序调用。

## 功能特性

- ✅ RESTful API 设计
- ✅ Swagger/OpenAPI 文档
- ✅ CORS 跨域支持
- ✅ 数据验证
- ✅ 健康检查端点
- ✅ 完整的 CRUD 操作示例

## API 端点

### 健康检查
| 方法 | 端点 | 描述 |
|------|------|------|
| GET | `/health` | 检查API服务状态 |

### 用户管理
| 方法 | 端点 | 描述 |
|------|------|------|
| GET | `/api/users` | 获取所有用户 |
| GET | `/api/users/{id}` | 根据ID获取用户 |
| POST | `/api/users` | 创建新用户 |
| PUT | `/api/users/{id}` | 更新用户信息 |
| DELETE | `/api/users/{id}` | 删除用户 |

### 产品管理
| 方法 | 端点 | 描述 |
|------|------|------|
| GET | `/api/products` | 获取所有产品（支持搜索） |
| GET | `/api/products/{id}` | 根据ID获取产品 |
| POST | `/api/products` | 创建新产品 |
| PUT | `/api/products/{id}` | 更新产品信息 |
| DELETE | `/api/products/{id}` | 删除产品 |

## 快速开始

### 1. 运行项目

```bash
cd MyNet8Api
dotnet run
```

### 2. 访问 Swagger 文档

启动后访问：
```
https://localhost:7001/swagger
```
或
```
http://localhost:5160/swagger
```

### 3. 测试 API

可以使用 `.http` 文件中的请求示例，或使用 curl：

```bash
# 获取所有用户
curl http://localhost:5160/api/users

# 创建新用户
curl -X POST http://localhost:5160/api/users \
  -H "Content-Type: application/json" \
  -d '{"name":"测试用户","email":"test@example.com","age":25}'

# 获取所有产品
curl http://localhost:5160/api/products

# 搜索产品
curl "http://localhost:5160/api/products?search=笔记本"
```

## 项目结构

```
MyNet8Api/
├── Controllers/
│   ├── UsersController.cs      # 用户管理API
│   └── ProductsController.cs   # 产品管理API
├── Models/
│   ├── User.cs                 # 用户模型
│   ├── Product.cs              # 产品模型
│   └── ApiResponse.cs          # 统一响应格式
├── Program.cs                  # 应用程序入口
├── appsettings.json            # 配置文件
└── MyNet8Api.http              # HTTP测试请求
```

## 跨域调用

本项目已配置 CORS，允许跨域调用：

### JavaScript/Fetch 示例

```javascript
// 获取用户列表
fetch('http://localhost:5160/api/users')
  .then(response => response.json())
  .then(data => console.log(data));

// 创建新用户
fetch('http://localhost:5160/api/users', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    name: '新用户',
    email: 'newuser@example.com',
    age: 30
  })
})
.then(response => response.json())
.then(data => console.log(data));
```

### Python/Requests 示例

```python
import requests

# 获取所有用户
response = requests.get('http://localhost:5160/api/users')
users = response.json()
print(users)

# 创建新用户
new_user = {
    "name": "Python用户",
    "email": "python@example.com",
    "age": 28
}
response = requests.post('http://localhost:5160/api/users', json=new_user)
print(response.json())
```

## 数据模型

### User (用户)
```json
{
  "id": 1,
  "name": "张三",
  "email": "zhangsan@example.com",
  "age": 25
}
```

### Product (产品)
```json
{
  "id": 1,
  "name": "笔记本电脑",
  "description": "高性能办公笔记本",
  "price": 5999.00,
  "stock": 100
}
```

## 开发说明

### 添加新的 API 控制器

1. 在 `Controllers` 文件夹中创建新的控制器类
2. 继承 `ControllerBase` 并添加 `[ApiController]` 和 `[Route]` 特性
3. 实现所需的 API 方法

### 配置 CORS

在 `Program.cs` 中修改 CORS 策略：

```csharp
// 允许所有来源（开发环境）
app.UseCors("AllowAll");

// 或允许特定来源（生产环境）
app.UseCors("AllowSpecificOrigins");
```

## 部署

### 发布项目

```bash
dotnet publish -c Release -o ./publish
```

### Docker 部署

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyNet8Api.dll"]
```

## 许可证

MIT License
