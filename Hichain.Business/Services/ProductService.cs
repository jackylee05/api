using Hichain.Business.Services;
using Hichain.Common.Utilities;
using Hichain.Entity.Data;
using Hichain.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hichain.Business.Services;

/// <summary>
/// 产品服务实现
/// </summary>
public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    public ProductService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 获取所有产品
    /// </summary>
    /// <returns>产品列表</returns>
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            return await _dbContext.Products
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "获取产品列表失败");
            throw;
        }
    }

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品</returns>
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Products
                .Where(p => p.Id == id && !p.IsDeleted)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "根据ID获取产品失败: {0}", id);
            throw;
        }
    }

    /// <summary>
    /// 搜索产品
    /// </summary>
    /// <param name="searchTerm">搜索词</param>
    /// <returns>产品列表</returns>
    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        try
        {
            return await _dbContext.Products
                .Where(p => !p.IsDeleted && (
                    p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "搜索产品失败: {0}", searchTerm);
            throw;
        }
    }

    /// <summary>
    /// 创建产品
    /// </summary>
    /// <param name="product">产品</param>
    /// <returns>创建的产品</returns>
    public async Task<Product> CreateProductAsync(Product product)
    {
        try
        {
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            
            return product;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "创建产品失败");
            throw;
        }
    }

    /// <summary>
    /// 更新产品
    /// </summary>
    /// <param name="product">产品</param>
    /// <returns>更新的产品</returns>
    public async Task<Product> UpdateProductAsync(Product product)
    {
        try
        {
            product.UpdatedAt = DateTime.UtcNow;
            
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            
            return product;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "更新产品失败: {0}", product.Id);
            throw;
        }
    }

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>是否成功</returns>
    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var product = await GetProductByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;
            
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "删除产品失败: {0}", id);
            throw;
        }
    }
}
