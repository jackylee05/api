# MyNet8Api - .NET 8 Web API

一个基于 .NET 8 的 Web API 示例项目，提供用户管理和产品管理的 RESTful API 接口，可供其他应用程序调用。

## 功能特性

- ✅ RESTful API 设计
- ✅ Swagger/OpenAPI 文档
- ✅ CORS 跨域支持
- ✅ JWT 认证与授权
- ✅ 角色权限控制
- ✅ 数据验证
- ✅ 健康检查端点
- ✅ 完整的 CRUD 操作示例

## API 端点

### 健康检查
| 方法 | 端点 | 描述 |
|------|------|------|
| GET | `/health` | 检查API服务状态（无需认证） |

### 认证管理
| 方法 | 端点 | 描述 |
|------|------|------|
| POST | `/api/auth/login` | 用户登录（无需认证） |
| POST | `/api/auth/register` | 用户注册（无需认证） |
| POST | `/api/auth/refresh` | 刷新令牌（无需认证） |

### 用户管理
| 方法 | 端点 | 描述 | 权限 |
|------|------|------|------|
| GET | `/api/users` | 获取所有用户 | 需认证 |
| GET | `/api/users/{id}` | 根据ID获取用户 | 需认证 |
| POST | `/api/users` | 创建新用户 | Admin角色 |
| PUT | `/api/users/{id}` | 更新用户信息 | Admin角色 |
| DELETE | `/api/users/{id}` | 删除用户 | Admin角色 |

### 产品管理
| 方法 | 端点 | 描述 | 权限 |
|------|------|------|------|
| GET | `/api/products` | 获取所有产品（支持搜索） | 需认证 |
| GET | `/api/products/{id}` | 根据ID获取产品 | 需认证 |
| POST | `/api/products` | 创建新产品 | Admin角色 |
| PUT | `/api/products/{id}` | 更新产品信息 | Admin角色 |
| DELETE | `/api/products/{id}` | 删除产品 | Admin角色 |

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

#### 登录获取令牌

**管理员账户：**
- 用户名：`admin`
- 密码：`admin123`

**普通用户账户：**
- 用户名：`user`
- 密码：`user123`

```bash
# 管理员登录
curl -X POST http://localhost:5160/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# 普通用户登录
curl -X POST http://localhost:5160/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"user","password":"user123"}'
```

#### 使用令牌访问受保护的 API

```bash
# 获取所有用户（需要认证）
curl http://localhost:5160/api/users \
  -H "Authorization: Bearer your-token-here"

# 创建新用户（需要Admin角色）
curl -X POST http://localhost:5160/api/users \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer your-token-here" \
  -d '{"name":"赵六","email":"zhaoliu@example.com","age":35}'

# 获取所有产品（需要认证）
curl http://localhost:5160/api/products \
  -H "Authorization: Bearer your-token-here"
```

## 项目结构

```
MyNet8Api/
├── Controllers/
│   ├── AuthController.cs       # 认证管理API
│   ├── UsersController.cs      # 用户管理API
│   └── ProductsController.cs   # 产品管理API
├── Models/
│   ├── User.cs                 # 用户模型
│   ├── Product.cs              # 产品模型
│   ├── Auth.cs                 # 认证相关模型
│   └── ApiResponse.cs          # 统一响应格式
├── Program.cs                  # 应用程序入口
├── appsettings.json            # 配置文件
└── MyNet8Api.http              # HTTP测试请求
```

## 跨域调用

本项目已配置 CORS，允许跨域调用：

### JavaScript/Fetch 示例

```javascript
// 登录获取令牌
async function login() {
  const response = await fetch('http://localhost:5160/api/auth/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      username: 'admin',
      password: 'admin123'
    })
  });
  const data = await response.json();
  return data.token;
}

// 使用令牌获取用户列表
async function getUsers() {
  const token = await login();
  const response = await fetch('http://localhost:5160/api/users', {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  const users = await response.json();
  console.log(users);
}

getUsers();
```

### Python/Requests 示例

```python
import requests

# 登录获取令牌
def login():
    response = requests.post('http://localhost:5160/api/auth/login', json={
        'username': 'admin',
        'password': 'admin123'
    })
    return response.json()['token']

# 使用令牌获取用户列表
def get_users():
    token = login()
    headers = {
        'Authorization': f'Bearer {token}'
    }
    response = requests.get('http://localhost:5160/api/users', headers=headers)
    users = response.json()
    print(users)

get_users()
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

### 认证响应
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "550e8400-e29b-41d4-a716-446655440000",
  "expiresAt": "2024-01-01T12:00:00Z",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@example.com",
    "name": "管理员",
    "roles": ["Admin"]
  }
}
```

## 开发说明

### 添加新的 API 控制器

1. 在 `Controllers` 文件夹中创建新的控制器类
2. 继承 `ControllerBase` 并添加 `[ApiController]` 和 `[Route]` 特性
3. 根据需要添加 `[Authorize]` 特性和角色要求

### 配置 JWT

在 `appsettings.json` 中修改 JWT 配置：

```json
"Jwt": {
  "Key": "your-super-secret-key-for-jwt-authentication",
  "Issuer": "MyNet8Api",
  "Audience": "MyNet8ApiUsers"
}
```

### 角色权限控制

- **[Authorize]** - 只需要认证
- **[Authorize(Roles = "Admin")]** - 需要 Admin 角色

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
