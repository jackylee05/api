using Microsoft.Extensions.Configuration;
using System.IO;

namespace Hichain.Common.Utilities;

/// <summary>
/// 配置帮助类
/// </summary>
public class ConfigurationHelper
{
    private static IConfiguration? _configuration;

    /// <summary>
    /// 获取配置实例
    /// </summary>
    public static IConfiguration Configuration
    {
        get
        {
            if (_configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile("appsettings.Development.json", optional: true);

                _configuration = builder.Build();
            }
            return _configuration;
        }
    }

    /// <summary>
    /// 获取配置值
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="key">配置键</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>配置值</returns>
    public static T Get<T>(string key, T defaultValue = default)
    {
        return Configuration.GetValue<T>(key, defaultValue);
    }

    /// <summary>
    /// 获取配置节
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="section">配置节</param>
    /// <returns>配置对象</returns>
    public static T GetSection<T>(string section)
    {
        return Configuration.GetSection(section).Get<T>();
    }
}
