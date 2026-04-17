///*********************************************************************
///Copyright (C) 2016-2019 56-cloud.com 
///Project Name          : 56SCM 
///Create By             : hkwong.wang@qq.com
///Create Date           : 2018-11-11
///Last Updated By       :
///Last Updated Date     :
///Version               :2018-11-11
///*********************************************************************

using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Hichain.SqlServerDatabaseEF.DbContexts
{
    /// <summary>
    /// Sql执行拦截器.
    /// </summary>
    public class DbCommandCustomInterceptor : DbCommandInterceptor
    {
        /// <summary>
        /// The NonQueryExecutingAsync.
        /// </summary>
        /// <param name="command">The command<see cref="DbCommand"/>.</param>
        /// <param name="eventData">The eventData<see cref="CommandEventData"/>.</param>
        /// <param name="result">The result<see cref="InterceptionResult{int}"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{InterceptionResult{int}}"/>.</returns>
        public async override Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        /// <summary>
        /// The NonQueryExecutedAsync.
        /// </summary>
        /// <param name="command">The command<see cref="DbCommand"/>.</param>
        /// <param name="eventData">The eventData<see cref="CommandExecutedEventData"/>.</param>
        /// <param name="result">The result<see cref="int"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async override Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalContext.SystemConfig.DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            int val = await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
            return val;
        }

        /// <summary>
        /// The ScalarExecutingAsync.
        /// </summary>
        /// <param name="command">The command<see cref="DbCommand"/>.</param>
        /// <param name="eventData">The eventData<see cref="CommandEventData"/>.</param>
        /// <param name="result">The result<see cref="InterceptionResult{object}"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{InterceptionResult{object}}"/>.</returns>
        public async override Task<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        /// <summary>
        /// The ScalarExecutedAsync.
        /// </summary>
        /// <param name="command">The command<see cref="DbCommand"/>.</param>
        /// <param name="eventData">The eventData<see cref="CommandExecutedEventData"/>.</param>
        /// <param name="result">The result<see cref="object"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        public async override Task<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalContext.SystemConfig.DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            var obj = await base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        /// <summary>
        /// The ReaderExecutingAsync.
        /// </summary>
        /// <param name="command">The command<see cref="DbCommand"/>.</param>
        /// <param name="eventData">The eventData<see cref="CommandEventData"/>.</param>
        /// <param name="result">The result<see cref="InterceptionResult{DbDataReader}"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{InterceptionResult{DbDataReader}}"/>.</returns>
        public async override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        /// <summary>
        /// The ReaderExecutedAsync.
        /// </summary>
        /// <param name="command">The command<see cref="DbCommand"/>.</param>
        /// <param name="eventData">The eventData<see cref="CommandExecutedEventData"/>.</param>
        /// <param name="result">The result<see cref="DbDataReader"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{DbDataReader}"/>.</returns>
        public async override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalContext.SystemConfig.DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            var reader = await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
            return reader;
        }
    }
}
