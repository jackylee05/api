using Hichain.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hichain.Entity.Data;

/// <summary>
/// 应用数据库上下文
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public DatabaseType DatabaseType { get; private set; }

    /// <summary>
    /// 用户表
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// 入库单表头
    /// </summary>
    public DbSet<Inbound> Inbound { get; set; }

    /// <summary>
    /// 入库单明细表体
    /// </summary>
    public DbSet<InboundPart> InboundParts { get; set; }


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">上下文选项</param>
    /// <param name="databaseType">数据库类型</param>
    public AppDbContext(DbContextOptions<AppDbContext> options, DatabaseType databaseType)
        : base(options)
    {
        DatabaseType = databaseType;
    }

    /// <summary>
    /// 模型配置
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置表名
        modelBuilder.Entity<User>().ToTable("Users");

        // 配置 WMS_Inbound 实体
        modelBuilder.Entity<Inbound>(entity =>
        {
            entity.ToTable("WMS_Inbound");
            entity.HasKey(e => e.InboundID);
            // 忽略导航属性（不映射到数据库）
            entity.Ignore(e => e.InboundParts);
        });

        // 配置 WMS_InboundPart 实体
        modelBuilder.Entity<InboundPart>(entity =>
        {
            entity.ToTable("InboundPart");
            entity.HasKey(e => e.InboundPartID);
        });

        // 种子数据
        SeedData(modelBuilder);
    }

    /// <summary>
    /// 种子数据
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    private void SeedData(ModelBuilder modelBuilder)
    {
        // 种子用户数据
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Password = "admin123", // 实际项目中应该加密
                Name = "管理员",
                Email = "admin@example.com",
                Age = 30,
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new User
            {
                Id = 2,
                Username = "user",
                Password = "user123", // 实际项目中应该加密
                Name = "普通用户",
                Email = "user@example.com",
                Age = 25,
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        );
    }
}
