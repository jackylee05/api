using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyNet8Api.Models;

namespace MyNet8Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "笔记本电脑", Description = "高性能办公笔记本", Price = 5999.00m, Stock = 100 },
        new Product { Id = 2, Name = "无线鼠标", Description = "人体工学设计", Price = 99.00m, Stock = 500 },
        new Product { Id = 3, Name = "机械键盘", Description = "RGB背光机械键盘", Price = 299.00m, Stock = 200 }
    };

    private static int _nextId = 4;

    /// <summary>
    /// 获取所有产品
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll([FromQuery] string? search = null)
    {
        var products = _products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            products = products.Where(p => 
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(products);
    }

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
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
    public ActionResult<Product> Create([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = new Product
        {
            Id = _nextId++,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock
        };

        _products.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// 更新产品信息
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<Product> Update(int id, [FromBody] UpdateProductRequest request)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的产品" });
        }

        product.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;
        product.Price = request.Price ?? product.Price;
        product.Stock = request.Stock ?? product.Stock;

        return Ok(product);
    }

    /// <summary>
    /// 删除产品
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的产品" });
        }

        _products.Remove(product);
        return NoContent();
    }
}
