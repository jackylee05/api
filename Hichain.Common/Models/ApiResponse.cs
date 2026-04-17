namespace Hichain.Common.Models;

/// <summary>
/// 统一API响应模型
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 成功响应
    /// </summary>
    public static ApiResponse<T> Ok(T? data, string message = "success")
    {
        return new ApiResponse<T>
        {
            Code = 200,
            Message = message,
            Data = data,
            Success = true
        };
    }

    /// <summary>
    /// 失败响应
    /// </summary>
    public static ApiResponse<T> Fail(int code, string message, T? data = default)
    {
        return new ApiResponse<T>
        {
            Code = code,
            Message = message,
            Data = data,
            Success = false
        };
    }
}

/// <summary>
/// 无数据的API响应模型
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    /// <summary>
    /// 成功响应（无数据）
    /// </summary>
    public static ApiResponse Ok(string message = "success")
    {
        return new ApiResponse
        {
            Code = 200,
            Message = message,
            Data = null,
            Success = true
        };
    }

    /// <summary>
    /// 失败响应（无数据）
    /// </summary>
    public static ApiResponse Fail(int code, string message)
    {
        return new ApiResponse
        {
            Code = code,
            Message = message,
            Data = null,
            Success = false
        };
    }
}
