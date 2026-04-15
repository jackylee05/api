using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hichain.Business.Services;
using Hichain.Entity.Entities;
using Hichain.Common.Models;

namespace Hichain.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 获取所有用户
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(ApiResponse<IEnumerable<User>>.Ok(users));
    }

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<User>>> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse<User>.Fail(404, $"未找到ID为 {id} 的用户"));
        }
        return Ok(ApiResponse<User>.Ok(user));
    }

    /// <summary>
    /// 创建新用户
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<User>>> Create([FromBody] User user)
    {
        var createdUser = await _userService.CreateUserAsync(user);
        return Ok(ApiResponse<User>.Ok(null, "用户创建成功"));
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<User>>> Update(int id, [FromBody] User user)
    {
        if (id != user.Id)
        {
            return BadRequest(ApiResponse<User>.Fail(400, "ID不匹配"));
        }

        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound(ApiResponse<User>.Fail(404, $"未找到ID为 {id} 的用户"));
        }

        var updatedUser = await _userService.UpdateUserAsync(user);
        return Ok(ApiResponse<User>.Ok(updatedUser, "用户更新成功"));
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
        {
            return NotFound(ApiResponse.Fail(404, $"未找到ID为 {id} 的用户"));
        }
        return Ok(ApiResponse.Ok("用户删除成功"));
    }
}
