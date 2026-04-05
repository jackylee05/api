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
    /// 产品表
    /// </summary>
    public DbSet<Product> Products { get; set; }

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
        modelBuilder.Entity<Product>().ToTable("Products");

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

        // 种子产品数据
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "笔记本电脑",
                Description = "高性能办公笔记本",
                Price = 5999.00m,
                Stock = 100,
                Category = "电子产品",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Id = 2,
                Name = "无线鼠标",
                Description = "人体工学设计",
                Price = 99.00m,
                Stock = 500,
                Category = "电子产品",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        );
    }
}
