///*********************************************************************
///Copyright (C) 2016-2019 56-cloud.com 
///Project Name          : 56SCM 
///Create By             : hkwong.wang@qq.com
///Create Date           : 2018-11-11
///Last Updated By       :
///Last Updated Date     :
///Version               :2018-11-11
///*********************************************************************


namespace Hichain.SqlServerDatabaseEF.DbContexts
{
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Defines the SqlServer.
        /// </summary>
        SqlServer,

        /// <summary>
        /// Defines the MySql.
        /// </summary>
        MySql,
        /// <summary>
        /// PostgreSQL
        /// </summary>
        PostgreSql,
        /// <summary>
        /// Defines the Oracle.
        /// </summary>
        Oracle
    }
}
