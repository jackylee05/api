using Hichain.Business.Services;
using Hichain.Common.Utilities;
using Hichain.Entity.Data;
using Hichain.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hichain.Business.Services;

/// <summary>
/// 用户服务实现
/// </summary>
public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 获取所有用户
    /// </summary>
    /// <returns>用户列表</returns>
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            return await _dbContext.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "获取用户列表失败");
            throw;
        }
    }

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户</returns>
    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Users
                .Where(u => u.Id == id && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "根据ID获取用户失败: {0}", id);
            throw;
        }
    }

    /// <summary>
    /// 根据用户名获取用户
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>用户</returns>
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        try
        {
            return await _dbContext.Users
                .Where(u => u.Username == username && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "根据用户名获取用户失败: {0}", username);
            throw;
        }
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="user">用户</param>
    /// <returns>创建的用户</returns>
    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            
            return user;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "创建用户失败");
            throw;
        }
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="user">用户</param>
    /// <returns>更新的用户</returns>
    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            user.UpdatedAt = DateTime.UtcNow;
            
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            
            return user;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "更新用户失败: {0}", user.Id);
            throw;
        }
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>是否成功</returns>
    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var user = await GetUserByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(ex, "删除用户失败: {0}", id);
            throw;
        }
    }
}
