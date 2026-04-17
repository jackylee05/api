using Hichain.Entity.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Hichain.SqlServerDatabaseEF.DbContexts
{
    public static class SqlAdaptersMapping
    {
        public static readonly Dictionary<DatabaseType, ISqlOperationsAdapter> SqlOperationAdapterMapping =
            new Dictionary<DatabaseType, ISqlOperationsAdapter>
            {
                {DatabaseType.SqlServer, new SqlServerAdapter()}
            };

        public static readonly Dictionary<DatabaseType, IQueryBuilderSpecialization> SqlQueryBuilderSpecializationMapping =
            new Dictionary<DatabaseType, IQueryBuilderSpecialization>
            {
                {DatabaseType.MySql, new MySqlDialect()},
                {DatabaseType.PostgreSql, new PostgreSqlDialect()},
                {DatabaseType.SqlServer, new SqlServerDialect()}
            };

        public static ISqlOperationsAdapter CreateBulkOperationsAdapter(DbContext context)
        {
            var providerType = GetDatabaseType(context);
            return SqlOperationAdapterMapping[providerType];
        }

        public static IQueryBuilderSpecialization GetAdapterDialect(DbContext context)
        {
            var providerType = GetDatabaseType(context);
            return GetAdapterDialect(providerType);
        }

        public static IQueryBuilderSpecialization GetAdapterDialect(DatabaseType providerType)
        {
            return SqlQueryBuilderSpecializationMapping[providerType];
        }

        public static DatabaseType GetDatabaseType(DbContext context)
        {
            if (context.Database.ProviderName.Contains(DatabaseType.MySql.ToString()))
                return DatabaseType.MySql;
            if (context.Database.ProviderName.Contains("PostgreSQL"))
                return DatabaseType.PostgreSql;
            return DatabaseType.SqlServer;
        }
    }
}
