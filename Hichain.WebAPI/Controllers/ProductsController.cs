using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hichain.Business.Services;
using Hichain.Entity.Entities;

namespace Hichain.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// 获取所有产品
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] string? search = null)
    {
        var products = string.IsNullOrEmpty(search)
            ? await _productService.GetAllProductsAsync()
            : await _productService.SearchProductsAsync(search);
        
        return Ok(products);
    }

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的产品" });
        }
        return Ok(product);
    }

    /// <summary>
    /// 创建新产品
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        var createdProduct = await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    /// <summary>
    /// 更新产品信息
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> Update(int id, [FromBody] Product product)
    {
        if (id != product.Id)
        {
            return BadRequest(new { Message = "ID不匹配" });
        }

        var existingProduct = await _productService.GetProductByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的产品" });
        }

        var updatedProduct = await _productService.UpdateProductAsync(product);
        return Ok(updatedProduct);
    }

    /// <summary>
    /// 删除产品
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _productService.DeleteProductAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的产品" });
        }
        return NoContent();
    }
}
