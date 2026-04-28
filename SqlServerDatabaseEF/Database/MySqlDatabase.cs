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
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hichain.Common.Utilities;

namespace Hichain.DataAccess.Data.EF
{
    /// <summary>
    /// Defines the <see cref="MySqlDatabase" />.
    /// </summary>
    public class MySqlDatabase : IDatabase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDatabase"/> class.
        /// </summary>
        /// <param name="connString">The connString<see cref="string"/>.</param>
        public MySqlDatabase(string connString)
        {
            dbContext = new MySqlDbContext(connString);
        }

        /// <summary>
        /// Gets or sets the dbContext
        /// 获取 当前使用的数据访问上下文对象.
        /// </summary>
        public DbContext dbContext { get; set; }

        /// <summary>
        /// Gets or sets the dbContextTransaction
        /// 事务对象.
        /// </summary>
        public IDbContextTransaction dbContextTransaction { get; set; }

        /// <summary>
        /// 事务开始.
        /// </summary>
        /// <returns>.</returns>
        public async Task<IDatabase> BeginTrans()
        {
            DbConnection dbConnection = dbContext.Database.GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                await dbConnection.OpenAsync();
            }
            dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
            return this;
        }

        /// <summary>
        /// 提交当前操作的结果.
        /// </summary>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> CommitTrans()
        {
            try
            {
                DbContextExtension.SetEntityDefaultValue(dbContext);
                int returnValue = await dbContext.SaveChangesAsync();
                if (dbContextTransaction != null)
                {
                    await dbContextTransaction.CommitAsync();
                    await this.Close();
                }
                else
                {
                    await this.Close();
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw new Exception("Data commit error:", ex);
            }
            finally
            {
                if (dbContextTransaction == null)
                {
                    await this.Close();
                }
            }
        }

        /// <summary>
        /// 把当前操作回滚成未提交状态.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task RollbackTrans()
        {
            await this.dbContextTransaction.RollbackAsync();
            await this.dbContextTransaction.DisposeAsync();
            await this.Close();
        }

        /// <summary>
        /// 关闭连接 内存回收.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Close()
        {
            await dbContext.DisposeAsync();
        }

        /// <summary>
        /// The ExecuteBySql.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> ExecuteBySql(string strSql)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(strSql);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(strSql);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }

        /// <summary>
        /// The ExecuteBySql.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }

        /// <summary>
        /// The ExecuteByProc.
        /// </summary>
        /// <param name="procName">The procName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> ExecuteByProc(string procName)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName));
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName));
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }

        /// <summary>
        /// The ExecuteByProc.
        /// </summary>
        /// <param name="procName">The procName<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Insert<T>(T entity) where T : class
        {
            dbContext.Entry<T>(entity).State = EntityState.Added;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entities">The entities<see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Insert<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Added;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// InsertBulk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="bulkconfig"></param>
        /// <returns></returns>
        public async Task<int> InsertBulk<T>(IEnumerable<T> entities, BulkConfig bulkconfig) where T : class
        {
            await dbContext.BulkInsertAsync<T>(entities.ToList(), bulkconfig, null);
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Delete<T>() where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName));
            }
            return -1;
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Delete<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Set<T>().Remove(entity);
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entities">The entities<see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Delete<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Set<T>().Attach(entity);
                dbContext.Set<T>().Remove(entity);
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbContext.Set<T>().Where(condition).BatchDeleteAsync();
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="keyValue">The keyValue<see cref="long"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Delete<T>(long keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="keyValue">The keyValue<see cref="long[]"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Delete<T>(long[] keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <param name="propertyValue">The propertyValue<see cref="long"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Delete<T>(string propertyName, long propertyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, propertyName, propertyValue));
            }
            return -1;
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Update<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            Hashtable props = DatabasesExtension.GetPropertyInfo<T>(entity);
            foreach (string item in props.Keys)
            {
                if (item == "Id")
                {
                    continue;
                }
                object value = dbContext.Entry(entity).Property(item).CurrentValue;
                if (value != null)
                {
                    dbContext.Entry(entity).Property(item).IsModified = true;
                }
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entities">The entities<see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Update<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                await this.Update(entity);
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// The UpdateAllField.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> UpdateAllField<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// The UpdatePortionField.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <param name="pfields">The pfields<see cref="string[]"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> UpdatePortionField<T>(T entity, string[] pfields) where T : class
        {
            if (pfields == null || pfields.Length == 0) return 0;
            dbContext.Set<T>().Attach(entity);
            Hashtable props = DatabasesExtension.GetPropertyInfo<T>(entity);
            foreach (string item in props.Keys)
            {
                if (item == "Id") continue;
                if (pfields.Any(r => string.Equals(r, item, StringComparison.InvariantCultureIgnoreCase)))
                    dbContext.Entry(entity).Property(item).IsModified = true;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            IEnumerable<T> entities = await dbContext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Update(entities) : 0;
        }

        /// <summary>
        /// The IQueryable.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="IQueryable{T}"/>.</returns>
        public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return dbContext.Set<T>().Where(condition);
        }

        /// <summary>
        /// The FindEntity.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="keyValue">The keyValue<see cref="object"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> FindEntity<T>(object keyValue) where T : class
        {
            return await dbContext.Set<T>().FindAsync(keyValue);
        }

        /// <summary>
        /// The FindEntity.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbContext.Set<T>().Where(condition).FirstOrDefaultAsync();
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        public async Task<IEnumerable<T>> FindList<T>() where T : class, new()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="orderby">The orderby<see cref="Func{T, object}"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        public async Task<IEnumerable<T>> FindList<T>(Func<T, object> orderby) where T : class, new()
        {
            var list = await dbContext.Set<T>().ToListAsync();
            list = list.OrderBy(orderby).ToList();
            return list;
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbContext.Set<T>().Where(condition).ToListAsync();
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        public async Task<IEnumerable<T>> FindList<T>(string strSql) where T : class
        {
            return await FindList<T>(strSql, null);
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                var reader = await new DbHelper(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return DatabasesExtension.IDataReaderToList<T>(reader);
            }
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, IEnumerable{T} list)}"/>.</returns>
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            var tempData = dbContext.Set<T>().AsQueryable();
            return await FindList<T>(tempData, sort, isAsc, pageSize, pageIndex);
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, IEnumerable{T} list)}"/>.</returns>
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(Expression<Func<T, bool>> condition, string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            var tempData = dbContext.Set<T>().Where(condition);
            return await FindList<T>(tempData, sort, isAsc, pageSize, pageIndex);
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, IEnumerable{T})}"/>.</returns>
        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            return await FindList<T>(strSql, null, sort, isAsc, pageSize, pageIndex);
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, IEnumerable{T})}"/>.</returns>
        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                DbHelper dbHelper = new DbHelper(dbContext, dbConnection);
                StringBuilder sb = new StringBuilder();
                sb.Append(DatabasePageExtension.MySqlPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
                object tempTotal = await dbHelper.ExecuteScalarAsync(CommandType.Text, DatabasePageExtension.GetCountSql(strSql), dbParameter);
                int total = tempTotal.ParseToInt();
                if (total > 0)
                {
                    var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    return (total, DatabasesExtension.IDataReaderToList<T>(reader));
                }
                else
                {
                    return (total, new List<T>());
                }
            }
        }

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tempData">The tempData<see cref="IQueryable{T}"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, IEnumerable{T} list)}"/>.</returns>
        private async Task<(int total, IEnumerable<T> list)> FindList<T>(IQueryable<T> tempData, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            tempData = DatabasesExtension.AppendSort<T>(tempData, sort, isAsc);
            var total = tempData.Count();
            if (total > 0)
            {
                tempData = tempData.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).AsQueryable();
                var list = await tempData.ToListAsync();
                return (total, list);
            }
            else
            {
                return (total, new List<T>());
            }
        }

        /// <summary>
        /// The FindTable.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{DataTable}"/>.</returns>
        public async Task<DataTable> FindTable(string strSql)
        {
            return await FindTable(strSql, null);
        }

        /// <summary>
        /// The FindTable.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{DataTable}"/>.</returns>
        public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                var reader = await new DbHelper(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return DatabasesExtension.IDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// The FindTable.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, DataTable)}"/>.</returns>
        public async Task<(int total, DataTable)> FindTable(string strSql, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            return await FindTable(strSql, null, sort, isAsc, pageSize, pageIndex);
        }

        /// <summary>
        /// The FindTable.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, DataTable)}"/>.</returns>
        public async Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                DbHelper dbHelper = new DbHelper(dbContext, dbConnection);
                StringBuilder sb = new StringBuilder();
                sb.Append(DatabasePageExtension.MySqlPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
                object tempTotal = await dbHelper.ExecuteScalarAsync(CommandType.Text, ("SELECT COUNT(1) FROM (" + strSql + ") T").ToLower(), dbParameter);
                int total = tempTotal.ParseToInt();
                if (total > 0)
                {
                    var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    DataTable resultTable = DatabasesExtension.IDataReaderToDataTable(reader);
                    return (total, resultTable);
                }
                else
                {
                    return (total, new DataTable());
                }
            }
        }

        /// <summary>
        /// The FindMaxSeq.
        /// </summary>
        /// <param name="seqcol">The seqcol<see cref="string"/>.</param>
        /// <param name="tablename">The tablename<see cref="string"/>.</param>
        /// <param name="strwhere">The strwhere<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        public async Task<object> FindMaxSeq(string seqcol, string tablename, string strwhere)
        {
            string strsql = string.Format(("SELECT MAX({0}) FROM {1} WHERE {2}").ToLower(), seqcol, tablename, strwhere);
            return await FindObject(strsql, null);
        }

        /// <summary>
        /// The FindObject.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        public async Task<object> FindObject(string strSql)
        {
            return await FindObject(strSql, null);
        }

        /// <summary>
        /// The FindObject.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        public async Task<object> FindObject(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                return await new DbHelper(dbContext, dbConnection).ExecuteScalarAsync(CommandType.Text, strSql, dbParameter);
            }
        }

        /// <summary>
        /// The FindObject.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> FindObject<T>(string strSql) where T : class
        {
            //var list = await dbContext.SqlQuery<T>(strSql);
            var list = await dbContext.Database
                     .SqlQueryRaw<T>(strSql)
                     .ToListAsync();
            return list.FirstOrDefault();
        }
    }
}
