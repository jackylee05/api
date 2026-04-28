///*********************************************************************
///Copyright (C) 2016-2019 56-cloud.com 
///Project Name          : 56SCM 
///Create By             : hkwong.wang@qq.com
///Create Date           : 2018-11-11
///Last Updated By       :
///Last Updated Date     :
///Version               :2018-11-11
///*********************************************************************


using System;
using System.Linq;
using Hichain.Common.Utilities;
using Hichain.DataAccess.Data.EF;
namespace Hichain.DataAccess.Data.Repository;


/// <summary>
/// Defines the <see cref="RepositoryFactory" />.
/// </summary>
public class RepositoryFactory
{
    /// <summary>
    /// The BaseRepository.
    /// </summary>
    /// <returns>The <see cref="Repository"/>.</returns>
    public Repository BaseRepository()
    {
        IDatabase database = null;
        string dbType = GlobalContext.SystemConfig.DBProvider;
        string dbConnectionString = GlobalContext.SystemConfig.DBConnectionString;
        switch (dbType)
        {
            case "SqlServer":
                DbHelper.DbType = DatabaseType.SqlServer;
                database = new SqlServerDatabase(dbConnectionString);
                break;
            case "MySql":
                DbHelper.DbType = DatabaseType.MySql;
                database = new MySqlDatabase(dbConnectionString);
                break;
            case "PostgreSql":
                DbHelper.DbType = DatabaseType.PostgreSql;
                database = new PostgreSqlDatabase(dbConnectionString);
                break;
            case "Oracle":
                DbHelper.DbType = DatabaseType.Oracle;
                break;
            default:
                 throw new Exception("未找到数据库配置");
        }
        return new Repository(database);
    }

    /// <summary>
    /// The BaseRepository.
    /// </summary>
    /// <param name="instance">The instance<see cref="string"/>.</param>
    /// <returns>The <see cref="Repository"/>.</returns>
    public Repository BaseRepository(string instance)
    {
        var instanceDB = GlobalContext.ConnConfigs.Where(r => r.Instance.ToLower() == instance.ToLower()).FirstOrDefault();
        if (string.IsNullOrEmpty(instance) || instanceDB == null) throw new Exception("instanceDB is System.NullReferenceException");
        IDatabase database = null;
        string dbType = instanceDB.ConnType;
        string dbConnectionString = instanceDB.ConnString;
        switch (dbType)
        {
            case "SqlServer":
                DbHelper.DbType = DatabaseType.SqlServer;
                database = new SqlServerDatabase(dbConnectionString);
                break;
            case "MySql":
                //DbHelper.DbType = DatabaseType.MySql;
                // database = new MySqlDatabase(dbConnectionString);
                break;
            case "PostgreSql":
                //DbHelper.DbType = DatabaseType.PostgreSql;
                //database = new PostgreSqlDatabase(dbConnectionString);
                break;
            case "Oracle":
                DbHelper.DbType = DatabaseType.Oracle;
                break;
            default:
                throw new Exception("未找到数据库配置");
        }
        return new Repository(database);
    }
}

