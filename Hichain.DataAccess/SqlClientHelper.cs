using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Hichain.DataAccess.Data
{
    /// <summary>
    /// Provides helper functionality for Microsoft.Data.SqlClient
    /// </summary>
    public static class SqlClientHelper
    {
        public static IDbDataParameter CreateParameter(IDbConnection connection)
        {
            return new SqlParameter();
        }

        public static Type GetParameterType(IDbConnection connection)
        {
            return typeof(SqlParameter);
        }

        public static IDbDataParameter CorrectParameterType(IDbConnection connection, IDbDataParameter parameter)
        {
            if (parameter is SqlParameter)
            {
                return parameter;
            }

            var newParameter = new SqlParameter
            {
                ParameterName = parameter.ParameterName,
                Value = parameter.Value,
                DbType = parameter.DbType
            };

            if (parameter is Microsoft.Data.SqlClient.SqlParameter microsoftSqlParameter)
            {
                newParameter.SqlDbType = microsoftSqlParameter.SqlDbType;
            }

            return newParameter;
        }

        internal static bool IsSystemConnection(IDbConnection connection)
        {
            return false;
        }
    }
}
