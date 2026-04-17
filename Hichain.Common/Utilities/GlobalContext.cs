using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Hichain.Common.Models;


namespace Hichain.Common.Utilities
{
    public class GlobalContext
    {
        /// <summary>
        /// Gets or sets the Services
        /// All registered service and class instance container. Which are used for dependency injection..
        /// </summary>
        public static IServiceCollection Services { get; set; }

        /// <summary>
        /// Gets or sets the ServiceProvider
        /// Configured service provider..
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Gets or sets the Configuration
        /// Configuration.
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the HostingEnvironment
        /// HostingEnvironment.
        /// </summary>
        public static IWebHostEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the SystemConfig
        /// SystemConfig.
        /// </summary>
        public static SystemConfig SystemConfig { get; set; }

        /// <summary>
        /// Gets or sets the ConnConfigs
        /// ConnConfigs.
        /// </summary>
        public static List<ConnConfig> ConnConfigs { get; set; }
        /// <summary>
        /// 插件配置
        /// </summary>
        public static List<PluginConfig> PluginConfigs { get; set; }

        static GlobalContext()
        {
            // Safe defaults to avoid null reference during early initialization
            Services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            ServiceProvider = Services.BuildServiceProvider();
            Configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
            SystemConfig = new SystemConfig();
            ConnConfigs = new List<ConnConfig>();
            PluginConfigs = new List<PluginConfig>();
        }

        /// <summary>
        /// Gets or sets the ServerStartRunTime
        /// ServerStartRunTime.
        /// </summary>
        public static DateTime ServerStartRunTime { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsFirstServerStart
        /// IsFirstServerStart.
        /// </summary>
        public static bool IsFirstServerStart
        {
            get
            {
                TimeSpan timeSpan = DateTime.Now - ServerStartRunTime;
                return timeSpan.TotalMinutes < 2 ? true : false;
            }
        }

        /// <summary>
        /// GetVersion.
        /// </summary>
        /// <returns>.</returns>
        public static string GetVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return version.Major + "." + version.Minor;
        }

        /// <summary>
        /// 程序启动时，记录目录.
        /// </summary>
        /// <param name="env">.</param>
        public static void LogWhenStart(IWebHostEnvironment env)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("程序启动");
            sb.Append(" |ContentRootPath:" + env.ContentRootPath);
            sb.Append(" |WebRootPath:" + env.WebRootPath);
            sb.Append(" |IsDevelopment:" + env.IsDevelopment());
            //LogHelper.Debug(sb.ToString());
        }

        /// <summary>
        /// 设置cache control.
        /// </summary>
        /// <param name="context">.</param>
        public static void SetCacheControl(StaticFileResponseContext context)
        {
            int second = 365 * 24 * 60 * 60;
            context.Context.Response.Headers.Add("Cache-Control", new[] { "public,max-age=" + second });
            context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
            context.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
