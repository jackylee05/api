using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hichain.Entity.Entities;

/// <summary>
/// 产品实体
/// </summary>
[Table("Products")]
public class Product : BaseEntity
{
    /// <summary>
    /// 产品名称
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 价格
    /// </summary>
    [Required]
    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }

    /// <summary>
    /// 库存
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;
}
