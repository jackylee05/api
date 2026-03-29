using System.ComponentModel.DataAnnotations;

namespace MyNet8Api.Models;

/// <summary>
/// 产品实体
/// </summary>
public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
}

/// <summary>
/// 创建产品请求
/// </summary>
public class CreateProductRequest
{
    [Required(ErrorMessage = "产品名称是必填项")]
    [StringLength(100, ErrorMessage = "产品名称长度不能超过100个字符")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "产品描述长度不能超过500个字符")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "价格是必填项")]
    [Range(0.01, 999999.99, ErrorMessage = "价格必须在0.01-999999.99之间")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "库存是必填项")]
    [Range(0, int.MaxValue, ErrorMessage = "库存不能为负数")]
    public int Stock { get; set; }
}

/// <summary>
/// 更新产品请求
/// </summary>
public class UpdateProductRequest
{
    [StringLength(100, ErrorMessage = "产品名称长度不能超过100个字符")]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "产品描述长度不能超过500个字符")]
    public string? Description { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "价格必须在0.01-999999.99之间")]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "库存不能为负数")]
    public int? Stock { get; set; }
}
