using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hichain.Business.Services;
using Hichain.Entity.Entities;

namespace Hichain.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的用户" });
        }
        return Ok(user);
    }

    /// <summary>
    /// 创建新用户
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> Create([FromBody] User user)
    {
        var createdUser = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> Update(int id, [FromBody] User user)
    {
        if (id != user.Id)
        {
            return BadRequest(new { Message = "ID不匹配" });
        }

        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的用户" });
        }

        var updatedUser = await _userService.UpdateUserAsync(user);
        return Ok(updatedUser);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的用户" });
        }
        return NoContent();
    }
}
