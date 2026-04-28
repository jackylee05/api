using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hichain.Business.Services;
using Hichain.Entity.Entities;
using Hichain.Common.Models;
using Hichain.Business.UserBLL;

namespace Hichain.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class UsersController : ControllerBase
{
    private UserBLL _bll = new UserBLL();

    public UsersController()
    {
    }
    /// <summary>
    /// 创建新用户
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<User>>> Create([FromBody] User user)
    {
        await _bll.CreateUserAsync(user);
        return Ok(ApiResponse<User>.Ok(null, "用户创建成功"));
    }
}
