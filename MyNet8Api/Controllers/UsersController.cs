using Microsoft.AspNetCore.Mvc;
using MyNet8Api.Models;

namespace MyNet8Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<User> _users = new()
    {
        new User { Id = 1, Name = "张三", Email = "zhangsan@example.com", Age = 25 },
        new User { Id = 2, Name = "李四", Email = "lisi@example.com", Age = 30 },
        new User { Id = 3, Name = "王五", Email = "wangwu@example.com", Age = 28 }
    };

    private static int _nextId = 4;

    /// <summary>
    /// 获取所有用户
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        return Ok(_users);
    }

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    [HttpGet("{id}")]
    public ActionResult<User> GetById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
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
    public ActionResult<User> Create([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            Id = _nextId++,
            Name = request.Name,
            Email = request.Email,
            Age = request.Age
        };

        _users.Add(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    [HttpPut("{id}")]
    public ActionResult<User> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的用户" });
        }

        user.Name = request.Name ?? user.Name;
        user.Email = request.Email ?? user.Email;
        user.Age = request.Age ?? user.Age;

        return Ok(user);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { Message = $"未找到ID为 {id} 的用户" });
        }

        _users.Remove(user);
        return NoContent();
    }
}
