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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Hichain.Common.Utilities;

namespace Hichain.DataAccess.Data.EF
{
    /// <summary>
    /// Defines the <see cref="DbContextExtension" />.
    /// </summary>
    public static class DbContextExtension
    {
        /// <summary>
        /// 拼接删除SQL语句.
        /// </summary>
        /// <param name="tableName">表名.</param>
        /// <returns>.</returns>
        public static string DeleteSql(string tableName)
        {
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + "");
            return strSql.ToString().ToLower();
        }

        /// <summary>
        /// 拼接删除SQL语句.
        /// </summary>
        /// <param name="tableName">表名.</param>
        /// <param name="propertyName">实体属性名称.</param>
        /// <param name="propertyValue">字段值：数组1,2,3,4,5,6.</param>
        /// <returns>.</returns>
        public static string DeleteSql(string tableName, string propertyName, long propertyValue)
        {
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + propertyName + " = " + propertyValue + "");
            return strSql.ToString().ToLower();
        }

        /// <summary>
        /// 拼接批量删除SQL语句.
        /// </summary>
        /// <param name="tableName">表名.</param>
        /// <param name="propertyName">实体属性名称.</param>
        /// <param name="propertyValue">字段值：数组1,2,3,4,5,6.</param>
        /// <returns>.</returns>
        public static string DeleteSql(string tableName, string propertyName, long[] propertyValue)
        {
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + propertyName + " IN (");
            for (long i = 0; i < propertyValue.Length; i++)
            {
                if (i == 0)
                {
                    strSql.Append(propertyValue[i]);
                }
                else
                {
                    strSql.Append("," + propertyValue[i]);
                }
            }
            strSql.Append(")");
            return strSql.ToString().ToLower();
        }

        /// <summary>
        /// Get underlying <see cref="DbConnection"/> for given DbContext, applying BulkConfig.UnderlyingConnection if provided.
        /// </summary>
        public static DbConnection GetUnderlyingConnection(this DbContext context, BulkConfig config)
        {
            var conn = context.Database.GetDbConnection();
            if (config?.UnderlyingConnection != null)
            {
                return config.UnderlyingConnection(conn);
            }
            return conn;
        }

        /// <summary>
        /// Get underlying <see cref="DbTransaction"/> for given IDbContextTransaction, applying BulkConfig.UnderlyingTransaction if provided.
        /// </summary>
        public static DbTransaction GetUnderlyingTransaction(this IDbContextTransaction transaction, BulkConfig config)
        {
            if (transaction == null) return null;
            var dbTran = transaction.GetDbTransaction();
            if (config?.UnderlyingTransaction != null)
            {
                return config.UnderlyingTransaction(dbTran);
            }
            return dbTran;
        }
            
        /// <summary>
        /// 获取实体映射对象.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="dbcontext">.</param>
        /// <returns>.</returns>
        public static IEntityType GetEntityType<T>(DbContext dbcontext) where T : class
        {
            return dbcontext.Model.FindEntityType(typeof(T));
        }

        /// <summary>
        /// 存储过程语句.
        /// </summary>
        /// <param name="procName">存储过程名称.</param>
        /// <param name="dbParameter">执行命令所需的sql语句对应参数.</param>
        /// <returns>.</returns>
        public static string BuilderProc(string procName, params DbParameter[] dbParameter)
        {
            StringBuilder strSql = new StringBuilder("exec " + procName);
            if (dbParameter != null)
            {
                foreach (var item in dbParameter)
                {
                    strSql.Append(" " + item + ",");
                }
                strSql = strSql.Remove(strSql.Length - 1, 1);
            }
            return strSql.ToString().ToLower();
        }

        /// <summary>
        /// The SetEntityDefaultValue.
        /// </summary>
        /// <param name="dbcontext">The dbcontext<see cref="DbContext"/>.</param>
        public static void SetEntityDefaultValue(DbContext dbcontext)
        {
            foreach (EntityEntry entry in dbcontext.ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
            {
                Type type = entry.Entity.GetType();
                PropertyInfo[] props = ReflectionHelper.GetProperties(type);
                foreach (PropertyInfo prop in props)
                {
                    object value = prop.GetValue(entry.Entity, null);
                    if (value == null)
                    {
                        string sType = string.Empty;
                        if (prop.PropertyType.GenericTypeArguments.Length > 0)
                        {
                            sType = prop.PropertyType.GenericTypeArguments[0].Name;
                        }
                        else
                        {
                            sType = prop.PropertyType.Name;
                        }
                        switch (sType)
                        {
                            case "Boolean":
                                prop.SetValue(entry.Entity, false);
                                break;
                            case "Char":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "SByte":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "Byte":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "Int16":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "UInt16":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "Int32":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "UInt32":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "Int64":
                                prop.SetValue(entry.Entity, (Int64)0);
                                break;
                            case "UInt64":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "Single":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "Double":
                                prop.SetValue(entry.Entity, 0);
                                break;
                            case "Decimal":
                                prop.SetValue(entry.Entity, (decimal)0);
                                break;
                            case "DateTime":
                                //prop.SetValue(entry.Entity, GlobalConstant.DefaultTime);
                                break;
                            case "String":
                                prop.SetValue(entry.Entity, string.Empty);
                                break;
                            default: break;
                        }
                    }
                    else if (value.ToString() == DateTime.MinValue.ToString())
                    {
                        // sql server datetime类型的的范围不到0001-01-01，所以转成1970-01-01
                        //prop.SetValue(entry.Entity, GlobalConstant.DefaultTime);
                    }
                }
            }
        }
    }
}
