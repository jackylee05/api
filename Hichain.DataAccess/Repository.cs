using Hichain.DataAccess;

namespace Hichain.DataAccess.Repository;

public class Repository<T> where T : class, new()
{
    private readonly DbHelper _db;

    public Repository(DbHelper db)
    {
        _db = db;
    }

    /// <summary>
    /// 插入实体并返回数据库生成的自增主键（SqlServer OUTPUT INSERTED.Id）。
    /// </summary>
    public async Task<int> Insert(T entity)
    {
        return await _db.InsertReturnIdentityAsync(entity);
    }

    public async Task<int> InsertBulk(List<T> list)
    {
        return await _db.InsertBulkAsync(list);
    }

    public async Task<int> Delete(object key)
    {
        return await _db.DeleteAsync<T>(key);
    }

    public async Task<T?> FindEntity(object key)
    {
        return await _db.FindAsync<T>(key);
    }

    public async Task<List<T>> FindList(string sql)
    {
        var result = await _db.QueryAsync<T>(sql);
        return result.ToList();
    }

    public async Task<object?> ExecuteBySql(string sql)
    {
        return await _db.ExecuteAsync(sql);
    }

    public async Task<int> UpdatePortionField(object key, Dictionary<string, object> fields)
    {
        return await _db.UpdatePartialAsync<T>(key, fields);
    }
}
