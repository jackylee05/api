using Hichain.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hichain.DataAccess.Data.EF
{
    public class SqlServerDbContext : DbContext, IDisposable
    {
        /// <summary>
        /// Gets or sets the ConnectionString.
        /// </summary>
        private string ConnectionString { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString<see cref="string"/>.</param>
        public SqlServerDbContext(string connectionString)
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
            optionsBuilder.UseSqlServer(ConnectionString, p => p.CommandTimeout(GlobalContext.SystemConfig.DBCommandTimeout)).UseLoggerFactory(loggerFactory);
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
            //foreach (var entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    PrimaryKeyConvention.SetPrimaryKey(modelBuilder, entity.Name);
            //    string currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
            //    modelBuilder.Entity(entity.Name).ToTable(currentTableName);
            //}
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    PrimaryKeyConvention.SetPrimaryKey(modelBuilder, entityType.ClrType);

            //    var tableName = entityType.GetTableName();

            //    modelBuilder.Entity(entityType.ClrType)
            //        .ToTable(tableName);
            //}
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                PrimaryKeyConvention.SetPrimaryKey(modelBuilder, clrType);

                var tableName = entityType.GetTableName();

                modelBuilder.Entity(clrType)
                    .ToTable(tableName);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
