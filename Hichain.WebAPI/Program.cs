using System.Reflection;
using Hichain.Common.Utilities;
using Hichain.Common.Models;
using Hichain.DataAccess;
using Hichain.Entity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NLog.Config;

var builder = WebApplication.CreateBuilder(args);

// 配置监听地址，类似旧项目的 UseUrls("http://*:5003")
builder.WebHost.UseUrls("http://*:5903");

// 配置日志，清除默认 Provider，设置最低级别，并接入 NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
// 显式从项目内容根目录加载 NLog 配置，避免 nlog.config 未拷贝到输出目录时无法生效
var nlogConfigPath = Path.Combine(builder.Environment.ContentRootPath, "nlog.config");
if (File.Exists(nlogConfigPath))
{
    NLog.LogManager.Configuration = new XmlLoggingConfiguration(nlogConfigPath);
}
builder.Host.UseNLog();

// 配置数据库连接
// 将 SystemConfig 从配置绑定并赋值到 GlobalContext
var _systemConfig = builder.Configuration.GetSection("SystemConfig").Get<SystemConfig>() ?? new SystemConfig();
GlobalContext.SystemConfig = _systemConfig;
GlobalContext.Configuration = builder.Configuration;
GlobalContext.Services = builder.Services;

// 绑定连接配置和插件配置到 GlobalContext
var _connConfigs = builder.Configuration.GetSection("ConnConfigs").Get<List<ConnConfig>>() ?? new List<ConnConfig>();
GlobalContext.ConnConfigs = _connConfigs;
var _pluginConfigs = builder.Configuration.GetSection("PluginConfigs").Get<List<PluginConfig>>() ?? new List<PluginConfig>();
GlobalContext.PluginConfigs = _pluginConfigs;

//var databaseType = builder.Configuration.GetValue<DatabaseType>("Database:Type", DatabaseType.SqlServer);
//var connectionString = GetConnectionString(builder.Configuration, databaseType);

// 让 Business 在“用户名/密码读取”阶段可以使用你新增的 DataAccess/Dapper
// 注意：Hichain.DataAccess/DbHelper 当前只实现了 SqlClient（SqlServer）
//if (databaseType == DatabaseType.SqlServer)
//{
//    builder.Services.AddSingleton<DbHelper>(_ => new DbHelper(connectionString));
//}

//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    switch (databaseType)
//    {
//        case DatabaseType.SqlServer:
//            options.UseSqlServer(connectionString);
//            break;
//        case DatabaseType.PostgreSQL:
//            options.UseNpgsql(connectionString);
//            break;
//    }
//});

// 配置 JWT 认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"] ?? "your-secret-key"
            ))
        };
    });

// 注册服务
//builder.Services.AddScoped<IUserService, UserBLL>();
//builder.Services.AddScoped<IProductService, ProductService>();
// 注册解析后的 DatabaseType，使其可以被 DI 注入到 AppDbContext 的构造函数中
// 使用非泛型重载以允许传递值类型（枚举）
//builder.Services.AddSingleton(typeof(DatabaseType), databaseType);

// 添加控制器
builder.Services.AddControllers();
// 配置 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 添加 XML 注释
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // 配置 JWT 认证
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT 认证（Bearer token）",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
// 配置 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();
// 添加全局异常处理中间件
app.UseMiddleware<Hichain.WebAPI.Middleware.GlobalExceptionMiddleware>();
// 确保即使框架日志较少，也能触发 NLog 写入文件
NLog.LogManager.GetCurrentClassLogger().Info("Hichain.WebAPI starting up.");
// Swagger：默认与 Development 一致；可在 appsettings 中设 "Swagger": { "Enable": true } 强制开启（如发布后调试）
var enableSwagger = app.Configuration.GetValue<bool?>("Swagger:Enable") ?? app.Environment.IsDevelopment();
if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hichain.WebAPI v1");
    });
}
// 统一走 HTTP 调试：不做 http -> https 重定向；监听端口见 UseUrls（当前 5903）
// app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// 根路径给个简单提示，避免访问 `/` 直接 404
app.MapGet("/", () => new
{
    Status = "Running",
    Endpoints = new[]
    {
        "/api/auth/login",
        "/api/users",
        "/swagger"
    }
});
app.Run();
//string GetConnectionString(IConfiguration configuration, DatabaseType databaseType)
//{
//    // 直接从配置中获取连接字符串，不使用硬编码的默认值
//    var connectionString = databaseType switch
//    {
//        DatabaseType.SqlServer => configuration["Database:SqlServer:ConnectionString"],
//        DatabaseType.PostgreSQL => configuration["Database:PostgreSQL:ConnectionString"],
//        _ => throw new NotSupportedException($"不支持的数据库类型: {databaseType}")
//    };
//    if (string.IsNullOrWhiteSpace(connectionString))
//    {
//        throw new InvalidOperationException($"数据库连接字符串未配置，请检查 appsettings.json 文件中的 Database:{databaseType}:ConnectionString 配置");
//    }
//    return connectionString;
//}
