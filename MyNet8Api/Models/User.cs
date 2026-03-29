using System.ComponentModel.DataAnnotations;

namespace MyNet8Api.Models;

/// <summary>
/// 用户实体
/// </summary>
public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public int Age { get; set; }
}

/// <summary>
/// 创建用户请求
/// </summary>
public class CreateUserRequest
{
    [Required(ErrorMessage = "姓名是必填项")]
    [StringLength(50, ErrorMessage = "姓名长度不能超过50个字符")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "邮箱是必填项")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    [Range(1, 150, ErrorMessage = "年龄必须在1-150之间")]
    public int Age { get; set; }
}

/// <summary>
/// 更新用户请求
/// </summary>
public class UpdateUserRequest
{
    [StringLength(50, ErrorMessage = "姓名长度不能超过50个字符")]
    public string? Name { get; set; }

    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string? Email { get; set; }

    [Range(1, 150, ErrorMessage = "年龄必须在1-150之间")]
    public int? Age { get; set; }
}
