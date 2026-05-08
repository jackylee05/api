using Hichain.Common.Models; 
using Hichain.Common.Utilities;
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Hichain.DataAccess.Data.EF
{
    /// <summary>
    /// Defines the <see cref="PostgreDbContext" />.
    /// </summary>
    public class PostgreDbContext : DbContext, IDisposable
    {
        /// <summary>
        /// Gets or sets the ConnectionString.
        /// </summary>
        private string ConnectionString { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString<see cref="string"/>.</param>
        public PostgreDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// The OnConfiguring.
        /// </summary>
        /// <param name="optionsBuilder">The optionsBuilder<see cref="DbContextOptionsBuilder"/>.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = new Microsoft.Extensions.Logging.LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseNpgsql(ConnectionString, p => p.CommandTimeout(GlobalContext.SystemConfig.DBCommandTimeout)).UseLoggerFactory(loggerFactory);
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
                modelBuilder.Entity(entity.Name).ToTable(currentTableName.ToLower());
                var properties = entity.GetProperties();
                foreach (var property in properties)
                {
                    modelBuilder.Entity(entity.Name).Property(property.Name).HasColumnName(property.Name.ToLower());
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
