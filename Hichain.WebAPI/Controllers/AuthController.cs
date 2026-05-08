using Hichain.Business;
using Hichain.Common.Models;
using Hichain.Common.Utilities;
using Hichain.Entity.Entities;
using Hichain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hichain.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private UserBLL _bll = new UserBLL();

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// 用户登录
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest req)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<AuthResponse>.Fail(400, "请求参数无效"));
        }
        // 从数据库查用户密码
        string dbPassword = string.Empty;
        // 时间校验（5分钟）
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        //if (Math.Abs(now - req.Timestamp) > 600)
        //{
        //    return BadRequest("请求已过期");
        //}
        // nonce去重（Redis）
        //string redisKey = $"nonce:{request.Nonce}";
        //bool exists = _redis.Exists(redisKey);
        //if (exists)
        //{
        //    return BadRequest("重复请求");
        //}
        //_redis.Set(redisKey, "1", TimeSpan.FromMinutes(5));
        // 验证用户
        var user = await _bll.GetUserByUsernameAsync(req.Username);
        if (user == null)
        {
            return Unauthorized(ApiResponse<AuthResponse>.Fail(401, "用户名不存在"));
        }
        dbPassword = user.Password;
        // 重新计算sign
        string serverSign = SignHelper.HmacSha256(req.Username + req.Timestamp + req.Nonce, dbPassword);
        if (!serverSign.Equals(req.Sign,
            StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("签名错误");
        }
        // 生成 JWT 令牌
        var token = GenerateJwtToken(user);
        var response = new AuthResponse
        {
            Token = token
        };
        return Ok(ApiResponse<AuthResponse>.Ok(response, "登录成功"));
    }

    /// <summary>
    /// 生成 JWT 令牌
    /// </summary>
    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:ExpirationHours"] ?? "1")),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

/// <summary>
/// 登录请求
/// </summary>
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public long Timestamp { get; set; }
    public string Nonce { get; set; }
    public string Sign { get; set; }
}

/// <summary>
/// 认证响应
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
}

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
