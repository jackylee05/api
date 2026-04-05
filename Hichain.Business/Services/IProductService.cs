using Hichain.Entity.Entities;

namespace Hichain.Business.Services;

/// <summary>
/// 产品服务接口
/// </summary>
public interface IProductService
{
    /// <summary>
    /// 获取所有产品
    /// </summary>
    /// <returns>产品列表</returns>
    Task<IEnumerable<Product>> GetAllProductsAsync();

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品</returns>
    Task<Product?> GetProductByIdAsync(int id);

    /// <summary>
    /// 搜索产品
    /// </summary>
    /// <param name="searchTerm">搜索词</param>
    /// <returns>产品列表</returns>
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);

    /// <summary>
    /// 创建产品
    /// </summary>
    /// <param name="product">产品</param>
    /// <returns>创建的产品</returns>
    Task<Product> CreateProductAsync(Product product);

    /// <summary>
    /// 更新产品
    /// </summary>
    /// <param name="product">产品</param>
    /// <returns>更新的产品</returns>
    Task<Product> UpdateProductAsync(Product product);

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteProductAsync(int id);
}
