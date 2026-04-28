namespace Hichain.Common.Models
{
    /// <summary>
    /// Defines the <see cref="SystemConfig" />.
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfig"/> class.
        /// </summary>
        public SystemConfig()
        {
            DBSlowSqlLogTime = 5;
        }

        /// <summary>
        /// Gets or sets a value indicating whether Demo
        /// 是否是Demo模式.
        /// </summary>
        public bool Demo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AuditLog
        /// 审计日志.
        /// </summary>
        public bool AuditLog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Debug
        /// 是否是调试模式.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether LoginMultiple
        /// 允许一个用户在多个电脑同时登录.
        /// </summary>
        public bool LoginMultiple { get; set; }

        /// <summary>
        /// Gets or sets the LoginProvider
        /// 登录信息保存方式 Cookie Session WebApi.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the SnowFlakeWorkerId
        /// Snow Flake Worker Id.
        /// </summary>
        public int SnowFlakeWorkerId { get; set; }

        /// <summary>
        /// Gets or sets the ApiSite
        /// api地址.
        /// </summary>
        public string ApiSite { get; set; }

        /// <summary>
        /// Gets or sets the AllowCorsSite
        /// 允许跨域的站点.
        /// </summary>
        public string AllowCorsSite { get; set; }

        /// <summary>
        /// Gets or sets the VirtualDirectory
        /// 网站虚拟目录.
        /// </summary>
        public string VirtualDirectory { get; set; }

        /// <summary>
        /// Gets or sets the MailHost
        /// SMTP服务器地址.
        /// </summary>
        public string MailHost { get; set; }

        /// <summary>
        /// Gets or sets the MailPort
        /// SMTP邮件端口.
        /// </summary>
        public string MailPort { get; set; }

        /// <summary>
        /// Gets or sets the MailUsername
        /// 邮件发送用户名.
        /// </summary>
        public string MailUsername { get; set; }

        /// <summary>
        /// Gets or sets the MailPassword
        /// 邮件发送密码.
        /// </summary>
        public string MailPassword { get; set; }

        /// <summary>
        /// Gets or sets the ErrorMailTo
        /// ErrorMailTo.
        /// </summary>
        public string ErrorMailTo { get; set; }

        /// <summary>
        /// Gets or sets the DBProvider
        /// 数据库类型.
        /// </summary>
        public string DBProvider { get; set; }

        /// <summary>
        /// Gets or sets the DBConnectionString
        /// 数据库链接.
        /// </summary>
        public string DBConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the DBCommandTimeout
        /// 数据库超时间（秒）.
        /// </summary>
        public int DBCommandTimeout { get; set; }

        /// <summary>
        /// Gets or sets the DBSlowSqlLogTime
        /// 慢查询记录Sql(秒),保存到文件以便分析.
        /// </summary>
        public int DBSlowSqlLogTime { get; set; }

        /// <summary>
        /// Gets or sets the DBBackup
        /// 数据库备份路径.
        /// </summary>
        public string DBBackup { get; set; }

        /// <summary>
        /// Gets or sets the CacheProvider
        /// 缓存方式.
        /// </summary>
        public string CacheProvider { get; set; }

        /// <summary>
        /// Gets or sets the RedisConnectionString
        /// Redis缓存链接方式.
        /// </summary>
        public string RedisConnectionString { get; set; }
    }

    /// <summary>
    /// 链接配置.
    /// </summary>
    public class ConnConfig
    {
        /// <summary>
        /// Gets or sets the Instance
        /// 数据库实例.
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// Gets or sets the ConnType
        /// 库类型.
        /// </summary>
        public string ConnType { get; set; }

        /// <summary>
        /// Gets or sets the ConnString
        /// 库链接.
        /// </summary>
        public string ConnString { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// 描述.
        /// </summary>
        public string Description { get; set; }
    }
    /// <summary>
    /// 插件配置
    /// </summary>
    public class PluginConfig
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// 插件描述.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 插件访问key
        /// </summary>
        public string AppKey { get; set; }
    }
    /// <summary>
    /// Defines the <see cref="JwtConfig" />.
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// Gets or sets the Secret
        /// 秘钥.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the ExpireMinus
        /// 过期分钟.
        /// </summary>
        public int ExpireMinus { get; set; }
    }
}
