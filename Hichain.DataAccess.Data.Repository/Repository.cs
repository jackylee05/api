using Hichain.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Hichain.DataAccess.Data;
using Hichain.Common.Models;

namespace Hichain.DataAccess.Data.Repository;

public class Repository 
{
    public IDatabase db;

    /// <summary>
    /// 插入实体并返回数据库生成的自增主键（SqlServer OUTPUT INSERTED.Id）。
    /// </summary>
    public Repository(IDatabase iDatabase)
    {
        this.db = iDatabase;
    }

    public async Task<Repository> BeginTrans()
    {
        await db.BeginTrans();
        return this;
    }


    /// <summary>
    /// The CommitTrans.
    /// </summary>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> CommitTrans()
    {
        return await db.CommitTrans();
    }

    /// <summary>
    /// The RollbackTrans.
    /// </summary>
    /// <returns>The <see cref="Task"/>.</returns>
    public async Task RollbackTrans()
    {
        await db.RollbackTrans();
    }

    /// <summary>
    /// The ExecuteBySql.
    /// </summary>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> ExecuteBySql(string strSql)
    {
        return await db.ExecuteBySql(strSql);
    }

    /// <summary>
    /// The ExecuteByProc.
    /// </summary>
    /// <param name="procName">The procName<see cref="string"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> ExecuteByProc(string procName)
    {
        return await db.ExecuteByProc(procName);
    }

    /// <summary>
    /// The ExecuteByProc.
    /// </summary>
    /// <param name="procName">The procName<see cref="string"/>.</param>
    /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter)
    {
        return await db.ExecuteByProc(procName, dbParameter);
    }
    /// <summary>
    /// The Insert.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="entity">The entity<see cref="T"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Insert<T>(T entity) where T : class
    {
        return await db.Insert<T>(entity);
    }

    /// <summary>
    /// The Insert.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="entity">The entity<see cref="List{T}"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Insert<T>(List<T> entity) where T : class
    {
        return await db.Insert<T>(entity);
    }

    /// <summary>
    /// InsertBulk
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="bulkconfig"></param>
    /// <returns></returns>
    public async Task<int> InsertBulk<T>(List<T> entity, BulkConfig bulkconfig) where T : class
    {
        return await db.InsertBulk<T>(entity, bulkconfig);
    }

    /// <summary>
    /// The Delete.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="entity">The entity<see cref="T"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Delete<T>(T entity) where T : class
    {
        return await db.Delete<T>(entity);
    }

    /// <summary>
    /// The Delete.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="entity">The entity<see cref="List{T}"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Delete<T>(List<T> entity) where T : class
    {
        return await db.Delete<T>(entity);
    }

    /// <summary>
    /// The Delete.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
    {
        return await db.Delete<T>(condition);
    }

    /// <summary>
    /// The Delete.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="id">The id<see cref="long"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Delete<T>(long id) where T : class
    {
        return await db.Delete<T>(id);
    }
    // <summary>
    /// The Delete.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="id">The id<see cref="long[]"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Delete<T>(long[] id) where T : class
    {
        return await db.Delete<T>(id);
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
        return await db.Delete<T>(propertyName, propertyValue);
    }
    /// <summary>
    /// The Update.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="entity">The entity<see cref="T"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Update<T>(T entity) where T : class
    {
        return await db.Update<T>(entity);
    }
    /// <summary>
    /// The Update.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="entity">The entity<see cref="List{T}"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Update<T>(List<T> entity) where T : class
    {
        return await db.Update<T>(entity);
    }
    /// <summary>
    /// The UpdateAllField.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="entity">The entity<see cref="T"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> UpdateAllField<T>(T entity) where T : class
    {
        return await db.UpdateAllField<T>(entity);
    }
    /// <summary>
    /// The Update.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
    /// <returns>The <see cref="Task{int}"/>.</returns>
    public async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
    {
        return await db.Update<T>(condition);
    }

    /// <summary>
    /// The IQueryable.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
    /// <returns>The <see cref="IQueryable{T}"/>.</returns>
    public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
    {
        return db.IQueryable<T>(condition);
    }

    /// <summary>
    /// The FindEntity.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="id">The id<see cref="long"/>.</param>
    /// <returns>The <see cref="Task{T}"/>.</returns>
    public async Task<T> FindEntity<T>(long id) where T : class
    {
        return await db.FindEntity<T>(id);
    }

    /// <summary>
    /// The FindEntity.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
    /// <returns>The <see cref="Task{T}"/>.</returns>
    public async Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
    {
        return await db.FindEntity<T>(condition);
    }

    /// <summary>
    /// The FindList.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
    public async Task<IEnumerable<T>> FindList<T>() where T : class, new()
    {
        return await db.FindList<T>();
    }

    /// <summary>
    /// The FindList.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
    /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
    public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
    {
        return await db.FindList<T>(condition);
    }

    /// <summary>
    /// The FindList.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
    public async Task<IEnumerable<T>> FindList<T>(string strSql) where T : class
    {
        return await db.FindList<T>(strSql);
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
        return await db.FindList<T>(strSql, dbParameter);
    }

    /// <summary>
    /// The FindList.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="pagination">The pagination<see cref="Pagination"/>.</param>
    /// <returns>The <see cref="Task{(int total, IEnumerable{T} list)}"/>.</returns>
    public async Task<(int total, IEnumerable<T> list)> FindList<T>(Pagination pagination) where T : class, new()
    {
        int total = pagination.TotalCount;
        var data = await db.FindList<T>(pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
        pagination.TotalCount = total;
        return data;
    }

    /// <summary>
    /// The FindList.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="condition">The condition<see cref="Expression{Func{T, bool}}"/>.</param>
    /// <param name="pagination">The pagination<see cref="Pagination"/>.</param>
    /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
    public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition, Pagination pagination) where T : class, new()
    {
        var data = await db.FindList<T>(condition, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
        pagination.TotalCount = data.total;
        return data.list;
    }

    /// <summary>
    /// The FindList.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <param name="pagination">The pagination<see cref="Pagination"/>.</param>
    /// <returns>The <see cref="Task{(int total, IEnumerable{T} list)}"/>.</returns>
    public async Task<(int total, IEnumerable<T> list)> FindList<T>(string strSql, Pagination pagination) where T : class
    {
        int total = pagination.TotalCount;
        var data = await db.FindList<T>(strSql, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
        pagination.TotalCount = total;
        return data;
    }

    /// <summary>
    /// The FindList.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
    /// <param name="pagination">The pagination<see cref="Pagination"/>.</param>
    /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
    public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter, Pagination pagination) where T : class
    {
        var data = await db.FindList<T>(strSql, dbParameter, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
        pagination.TotalCount = data.total;
        return data.Item2;
    }

    /// <summary>
    /// The FindTable.
    /// </summary>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <returns>The <see cref="Task{DataTable}"/>.</returns>
    public async Task<DataTable> FindTable(string strSql)
    {
        return await db.FindTable(strSql);
    }

    /// <summary>
    /// The FindTable.
    /// </summary>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
    /// <returns>The <see cref="Task{DataTable}"/>.</returns>
    public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter)
    {
        return await db.FindTable(strSql, dbParameter);
    }

    /// <summary>
    /// The FindTable.
    /// </summary>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <param name="pagination">The pagination<see cref="Pagination"/>.</param>
    /// <returns>The <see cref="Task{DataTable}"/>.</returns>
    public async Task<DataTable> FindTable(string strSql, Pagination pagination)
    {
        var data = await db.FindTable(strSql, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
        pagination.TotalCount = data.total;
        return data.Item2;
    }

    /// <summary>
    /// The FindTable.
    /// </summary>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
    /// <param name="pagination">The pagination<see cref="Pagination"/>.</param>
    /// <returns>The <see cref="Task{DataTable}"/>.</returns>
    public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter, Pagination pagination)
    {
        var data = await db.FindTable(strSql, dbParameter, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
        pagination.TotalCount = data.total;
        return data.Item2;
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
        return await db.FindMaxSeq(seqcol, tablename, strwhere);
    }

    /// <summary>
    /// The FindObject.
    /// </summary>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <returns>The <see cref="Task{object}"/>.</returns>
    public async Task<object> FindObject(string strSql)
    {
        return await db.FindObject(strSql);
    }

    /// <summary>
    /// The FindObject.
    /// </summary>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
    /// <returns>The <see cref="Task{object}"/>.</returns>
    public async Task<object> FindObject(string strSql, DbParameter[] dbParameter)
    {
        return await db.FindObject(strSql, dbParameter);
    }

    /// <summary>
    /// The FindObject.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="strSql">The strSql<see cref="string"/>.</param>
    /// <returns>The <see cref="Task{T}"/>.</returns>
    public async Task<T> FindObject<T>(string strSql) where T : class
    {
        return await db.FindObject<T>(strSql);
    }
}
