using Hichain.Entity.Entities;

namespace Hichain.Business.Services;

/// <summary>
/// 用户服务接口
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 获取所有用户
    /// </summary>
    /// <returns>用户列表</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户</returns>
    Task<User?> GetUserByIdAsync(int id);

    /// <summary>
    /// 根据用户名获取用户
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>用户</returns>
    Task<User?> GetUserByUsernameAsync(string username);

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="user">用户</param>
    /// <returns>创建的用户</returns>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="user">用户</param>
    /// <returns>更新的用户</returns>
    Task<User> UpdateUserAsync(User user);

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteUserAsync(int id);
}
