using Serilog;
using Serilog.Events;

namespace Hichain.Common.Utilities;

/// <summary>
/// 日志帮助类
/// </summary>
public static class LoggerHelper
{
    static LoggerHelper()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    /// <summary>
    /// 信息日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    public static void Information(string message, params object[] args)
    {
        Log.Information(message, args);
    }

    /// <summary>
    /// 警告日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    public static void Warning(string message, params object[] args)
    {
        Log.Warning(message, args);
    }

    /// <summary>
    /// 错误日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    public static void Error(string message, params object[] args)
    {
        Log.Error(message, args);
    }

    /// <summary>
    /// 错误日志
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    public static void Error(Exception exception, string message, params object[] args)
    {
        Log.Error(exception, message, args);
    }

    /// <summary>
    /// 调试日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    public static void Debug(string message, params object[] args)
    {
        Log.Debug(message, args);
    }

    /// <summary>
    /// 致命日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    public static void Fatal(string message, params object[] args)
    {
        Log.Fatal(message, args);
    }
}
