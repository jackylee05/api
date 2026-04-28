///*********************************************************************
///Copyright (C) 2016-2019 56-cloud.com 
///Project Name          : 56SCM 
///Create By             : hkwong.wang@qq.com
///Create Date           : 2018-11-11
///Last Updated By       :
///Last Updated Date     :
///Version               :2018-11-11
///*********************************************************************
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Hichain.Common.Utilities;

namespace Hichain.DataAccess.Data.EF
{
    /// <summary>
    /// Defines the <see cref="MySqlDbContext" />.
    /// </summary>
    public class MySqlDbContext : DbContext, IDisposable
    {
        /// <summary>
        /// Gets or sets the ConnectionString.
        /// </summary>
        private string ConnectionString { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString<see cref="string"/>.</param>
        public MySqlDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// The OnConfiguring.
        /// </summary>
        /// <param name="optionsBuilder">The optionsBuilder<see cref="DbContextOptionsBuilder"/>.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var loggerFactory = new Microsoft.Extensions.Logging.LoggerFactory();
            //loggerFactory.AddProvider(new EFLoggerProvider());
            //optionsBuilder.EnableSensitiveDataLogging(true);
            //optionsBuilder.UseMySql(ConnectionString, p => p.CommandTimeout(GlobalContext.SystemConfig.DBCommandTimeout)).UseLoggerFactory(loggerFactory);
            //optionsBuilder.AddInterceptors(new DbCommandCustomInterceptor());
            var loggerFactory = new Microsoft.Extensions.Logging.LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            // 1. 定义 ServerVersion (自动检测)
            // 注意：这会在首次连接时自动获取数据库版本
            var serverVersion = ServerVersion.AutoDetect(ConnectionString);
            // 2. 修改 UseMySql 的调用方式
            optionsBuilder.UseMySql(ConnectionString, serverVersion, p =>
            {
                p.CommandTimeout(GlobalContext.SystemConfig.DBCommandTimeout);
            })
            .UseLoggerFactory(loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.AddInterceptors(new DbCommandCustomInterceptor());
        }

        /// <summary>
        /// The OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Assembly entityAssembly = Assembly.Load(new AssemblyName("Hichain.Entity"));
            IEnumerable<Type> typesToRegister = entityAssembly.GetTypes().Where(p => !string.IsNullOrEmpty(p.Namespace))
                                                                         .Where(p => !string.IsNullOrEmpty(p.GetCustomAttribute<TableAttribute>()?.Name));
            foreach (Type type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Model.AddEntityType(type);
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entity.ClrType;
                PrimaryKeyConvention.SetPrimaryKey(modelBuilder, clrType);
                string currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
                modelBuilder.Entity(entity.Name).ToTable(currentTableName);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
