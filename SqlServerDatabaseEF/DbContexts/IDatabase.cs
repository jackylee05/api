using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hichain.SqlServerDatabaseEF.DbContexts;

namespace Hichain.SqlServerDatabaseEF.DbContexts
{
    public interface IDatabase
    {
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
        /// The BeginTrans.
        /// </summary>
        /// <returns>The <see cref="Task{IDatabase}"/>.</returns>
        Task<IDatabase> BeginTrans();

        /// <summary>
        /// The CommitTrans.
        /// </summary>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> CommitTrans();

        /// <summary>
        /// The RollbackTrans.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RollbackTrans();

        /// <summary>
        /// The Close.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Close();

        /// <summary>
        /// The ExecuteBySql.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> ExecuteBySql(string strSql);

        /// <summary>
        /// The ExecuteBySql.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter);

        /// <summary>
        /// The ExecuteByProc.
        /// </summary>
        /// <param name="procName">The procName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> ExecuteByProc(string procName);

        /// <summary>
        /// The ExecuteByProc.
        /// </summary>
        /// <param name="procName">The procName<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> ExecuteByProc(string procName, DbParameter[] dbParameter);

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Insert<T>(T entity) where T : class;

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entities">The entities<see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Insert<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// InsertBulk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="bulkConfig"></param>
        /// <returns></returns>
        Task<int> InsertBulk<T>(IEnumerable<T> entities, BulkConfig bulkconfig) where T : class;
        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Delete<T>() where T : class;

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Delete<T>(T entity) where T : class;

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entities">The entities<see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Delete<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="id">The id<see cref="long"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Delete<T>(long id) where T : class;

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="id">The id<see cref="long[]"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Delete<T>(long[] id) where T : class;

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <param name="propertyValue">The propertyValue<see cref="long"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Delete<T>(string propertyName, long propertyValue) where T : class;

        /// <summary>
        /// The Update.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Update<T>(T entity) where T : class;

        /// <summary>
        /// The Update.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entities">The entities<see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Update<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// The UpdateAllField.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> UpdateAllField<T>(T entity) where T : class;

        /// <summary>
        /// The UpdatePortionField.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <param name="pfields">The pfields<see cref="string[]"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> UpdatePortionField<T>(T entity, string[] pfields) where T : class;

        /// <summary>
        /// The Update.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// The IQueryable.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="IQueryable{T}"/>.</returns>
        IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// The FindEntity.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="KeyValue">The KeyValue<see cref="object"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        Task<T> FindEntity<T>(object KeyValue) where T : class;

        /// <summary>
        /// The FindEntity.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> FindList<T>() where T : class, new();

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="orderby">The orderby<see cref="Func{T, object}"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> FindList<T>(Func<T, object> orderby) where T : class, new();

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> FindList<T>(string strSql) where T : class;

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class;

        /// <summary>
        /// The FindList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, IEnumerable{T} list)}"/>.</returns>
        Task<(int total, IEnumerable<T> list)> FindList<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new();

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
        Task<(int total, IEnumerable<T> list)> FindList<T>(Expression<Func<T, bool>> condition, string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new();

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
        Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, string sort, bool isAsc, int pageSize, int pageIndex) where T : class;

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
        Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex) where T : class;

        /// <summary>
        /// The FindTable.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{DataTable}"/>.</returns>
        Task<DataTable> FindTable(string strSql);

        /// <summary>
        /// The FindTable.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{DataTable}"/>.</returns>
        Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter);

        /// <summary>
        /// The FindTable.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{(int total, DataTable)}"/>.</returns>
        Task<(int total, DataTable)> FindTable(string strSql, string sort, bool isAsc, int pageSize, int pageIndex);

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
        Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex);

        /// <summary>
        /// The FindMaxSeq.
        /// </summary>
        /// <param name="seqcol">The seqcol<see cref="string"/>.</param>
        /// <param name="tablename">The tablename<see cref="string"/>.</param>
        /// <param name="strwhere">The strwhere<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        Task<object> FindMaxSeq(string seqcol, string tablename, string strwhere);

        /// <summary>
        /// The FindObject.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        Task<object> FindObject(string strSql);

        /// <summary>
        /// The FindObject.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        Task<object> FindObject(string strSql, DbParameter[] dbParameter);

        /// <summary>
        /// The FindObject.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        Task<T> FindObject<T>(string strSql) where T : class;
    }
}
