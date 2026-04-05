using System.ComponentModel.DataAnnotations;

namespace Hichain.Entity.Entities;

/// <summary>
/// 用户实体
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 年龄
    /// </summary>
    [Range(1, 150)]
    public int Age { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Role { get; set; } = string.Empty;
}
