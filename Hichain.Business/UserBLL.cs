using GLSCM.Common.Util;
using Hichain.Services;
using Hichain.Common.Utilities;
using Hichain.Entity.Data;
using Hichain.Entity.Entities;
namespace Hichain.Business;
/// <summary>
/// 用户服务实现
/// </summary>
public class UserBLL
{
    private UserService _service = null;

    public UserBLL()
    {
        _service = new UserService();
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
            return await _service.GetEntity(username);
        }
        catch (Exception ex)
        {
            LogHelper.Error("根据用户名获取用户失败: " + username, ex);
            throw;
        }
    }
    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.IsDeleted = false;
            await _service.CreateUserAsync(user);
            return null;
        }
        catch (Exception ex)
        {
            WeixinConfig wcfg = new WeixinConfig();
            WeixinPushHelp.SendToMessage(wcfg, "创建用户报错："+ex.Message);
            LogHelper.Error("创建用户失败", ex);
            throw;
        }
    }
}
