using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hichain.Common.Utilities
{
    public static class SignHelper
    {
        /// <summary>
        /// 生成随机串
        /// </summary>
        public static string GenerateNonce()
        {
            return Guid.NewGuid().ToString("N");
        }
        /// <summary>
        /// Unix时间戳
        /// </summary>
        public static long GetTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        /// <summary>
        /// 生成签名
        /// </summary>
        public static string HmacSha256(string data,string secret)
        {
            using var hmac =
                new HMACSHA256(
                    Encoding.UTF8.GetBytes(secret));
            byte[] hash =
                hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(data));
            return Convert.ToHexString(hash).ToLower();
        }
    }
}
