using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace Hichain.Entity.Data;

/// <summary>
/// 数据库上下文工厂
/// </summary>
public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// 创建数据库上下文
    /// </summary>
    /// <param name="args">参数</param>
    /// <returns>数据库上下文</returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = LoadConfiguration();
        var databaseType = configuration.GetValue<DatabaseType>("Database:Type", DatabaseType.SqlServer);
        var connectionString = GetConnectionString(configuration, databaseType);

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        switch (databaseType)
        {
            case DatabaseType.SqlServer:
                optionsBuilder.UseSqlServer(connectionString);
                break;
            case DatabaseType.PostgreSQL:
                optionsBuilder.UseNpgsql(connectionString);
                break;
        }

        return new AppDbContext(optionsBuilder.Options, databaseType);
    }

    private static IConfiguration LoadConfiguration()
    {
        // 设计期（如 `dotnet ef`）当前目录不一定是 WebAPI 项目目录，
        // 因此这里显式优先加载 Hichain.WebAPI 的 appsettings.json。
        var basePath = Directory.GetCurrentDirectory();

        return new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(Path.Combine("Hichain.WebAPI", "appsettings.json"), optional: true, reloadOnChange: false)
            .AddJsonFile(Path.Combine("Hichain.WebAPI", "appsettings.Development.json"), optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false) // 兼容旧行为
            .Build();
    }

    /// <summary>
    /// 获取连接字符串
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="databaseType">数据库类型</param>
    /// <returns>连接字符串</returns>
    private static string GetConnectionString(IConfiguration configuration, DatabaseType databaseType)
    {
        // 直接从配置中获取连接字符串，不使用硬编码的默认值
        var connectionString = databaseType switch
        {
            DatabaseType.SqlServer => configuration["Database:SqlServer:ConnectionString"],
            DatabaseType.PostgreSQL => configuration["Database:PostgreSQL:ConnectionString"],
            _ => throw new NotSupportedException($"不支持的数据库类型: {databaseType}")
        };

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"数据库连接字符串未配置，请检查 appsettings.json 文件中的 Database:{databaseType}:ConnectionString 配置");
        }

        return connectionString;
    }
}
