using Hichain.Business.Services;
using Hichain.Common.Utilities;
using Hichain.DataAccess;
using Hichain.DataAccess.Repository;
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
    private readonly DbHelper? _dbHelper;
    private readonly Repository<User>? _userRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 构造函数（允许用 DataAccess/Dapper 查询用户，用于用户名/密码读取）
    /// </summary>
    public UserService(AppDbContext dbContext, DbHelper dbHelper) : this(dbContext)
    {
        _dbHelper = dbHelper;
        _userRepository = new Repository<User>(dbHelper);
    }

    /// <summary>
    /// 获取所有用户
    /// </summary>
    /// <returns>用户列表</returns>
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            if (_dbHelper is not null)
            {
                // 注意：EF 使用表名 Users（复数），因此这里也显式写 Users
                const string sql = @"SELECT Id, Username, Password, Name, Email, Age, Role, CreatedAt, UpdatedAt, IsDeleted
                                     FROM Users
                                     WHERE IsDeleted = 0";
                return await _dbHelper.QueryAsync<User>(sql);
            }

            return await _dbContext.Users.Where(u => !u.IsDeleted).ToListAsync();
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
            if (_dbHelper is not null)
            {
                const string sql = @"SELECT TOP 1 Id, Username, Password, Name, Email, Age, Role, CreatedAt, UpdatedAt, IsDeleted
                                     FROM Users
                                     WHERE Id = @Id AND IsDeleted = 0";
                return await _dbHelper.QueryFirstAsync<User>(sql, new { Id = id });
            }

            return await _dbContext.Users.Where(u => u.Id == id && !u.IsDeleted).FirstOrDefaultAsync();
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
            if (_dbHelper is not null)
            {
                const string sql = @"SELECT TOP 1 Id, Username, Password, Name, Email, Age, Role, CreatedAt, UpdatedAt, IsDeleted
                                     FROM Users
                                     WHERE Username = @Username AND IsDeleted = 0";
                return await _dbHelper.QueryFirstAsync<User>(sql, new { Username = username });
            }

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
            user.IsDeleted = false;

            if (_userRepository is not null)
            {
                user.Id = await _userRepository.Insert(user);
                return user;
            }

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
