using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hichain.SqlServerDatabaseEF.DbContexts
{
    public interface ISqlOperationsAdapter
    {
        void Insert<T>(DbContext context, Type type, IList<T> entities, TableInfoEx tableInfo, Action<decimal> progress);

        Task InsertAsync<T>(DbContext context, Type type, IList<T> entities, TableInfoEx tableInfo, Action<decimal> progress, CancellationToken cancellationToken);

        void Merge<T>(DbContext context, Type type, IList<T> entities, TableInfoEx tableInfo, OperationType operationType, Action<decimal> progress) where T : class;

        Task MergeAsync<T>(DbContext context, Type type, IList<T> entities, TableInfoEx tableInfo, OperationType operationType, Action<decimal> progress, CancellationToken cancellationToken) where T : class;

        void Read<T>(DbContext context, Type type, IList<T> entities, TableInfoEx tableInfo, Action<decimal> progress) where T : class;

        Task ReadAsync<T>(DbContext context, Type type, IList<T> entities, TableInfoEx tableInfo, Action<decimal> progress, CancellationToken cancellationToken) where T : class;

        void Truncate(DbContext context, TableInfoEx tableInfo);
        Task TruncateAsync(DbContext context, TableInfoEx tableInfo, CancellationToken cancellationToken);
    }
}
