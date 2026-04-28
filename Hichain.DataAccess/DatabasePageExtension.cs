///*********************************************************************
///Copyright (C) 2016-2019 56-cloud.com 
///Project Name          : 56SCM 
///Create By             : hkwong.wang@qq.com
///Create Date           : 2018-11-11
///Last Updated By       :
///Last Updated Date     :
///Version               :2018-11-11
///*********************************************************************

using System.Data.Common;
using System.Text;

namespace Hichain.DataAccess.Data
{
    /// <summary>
    /// Defines the <see cref="DatabasePageExtension" />.
    /// </summary>
    public class DatabasePageExtension
    {
        /// <summary>
        /// The SqlPageSql.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public static StringBuilder SqlPageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            int num1 = (pageIndex) * pageSize;
            string OrderBy = "";
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToUpper().IndexOf("ASC") + sort.ToUpper().IndexOf("DESC") > 0)
                {
                    OrderBy = " ORDER BY " + sort;
                }
                else
                {
                    OrderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            else
            {
                OrderBy = "ORDERE BY (SELECT 0)";
            }
            sb.Append("SELECT * FROM (SELECT ROW_NUMBER() Over (" + OrderBy + ")");
            sb.Append(" AS ROWNUM, * From (" + strSql + ") t ) AS N WHERE ROWNUM > " + num + " AND ROWNUM <= " + num1 + "");
            return sb;
        }

        /// <summary>
        /// The OraclePageSql.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public static StringBuilder OraclePageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            int num1 = (pageIndex) * pageSize;
            string OrderBy = "";
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToUpper().IndexOf("ASC") + sort.ToUpper().IndexOf("DESC") > 0)
                {
                    OrderBy = " ORDER BY " + sort;
                }
                else
                {
                    OrderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append("SELECT * From (SELECT ROWNUM AS n,");
            sb.Append(" T.* From (" + strSql + OrderBy + ") t )  N WHERE n > " + num + " AND n <= " + num1 + "");
            return sb;
        }

        /// <summary>
        /// The MySqlPageSql.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <param name="dbParameter">The dbParameter<see cref="DbParameter[]"/>.</param>
        /// <param name="sort">The sort<see cref="string"/>.</param>
        /// <param name="isAsc">The isAsc<see cref="bool"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public static StringBuilder MySqlPageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            string OrderBy = "";
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToUpper().IndexOf("ASC") + sort.ToUpper().IndexOf("DESC") > 0)
                {
                    OrderBy = " ORDER BY " + sort;
                }
                else
                {
                    OrderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append(strSql + OrderBy);
            sb.Append(" LIMIT " + num + "," + pageSize + "");
            return sb;
        }
        /// <summary>
        /// PostgreSqlPageSql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dbParameter"></param>
        /// <param name="sort"></param>
        /// <param name="isAsc"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static StringBuilder PostgreSqlPageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            string OrderBy = "";
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToUpper().IndexOf("ASC") + sort.ToUpper().IndexOf("DESC") > 0)
                {
                    OrderBy = " ORDER BY " + sort;
                }
                else
                {
                    OrderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append(strSql + OrderBy);
            sb.Append(" LIMIT " + pageSize + " offset " + num);
            return sb;
        }
        /// <summary>
        /// The GetCountSql.
        /// </summary>
        /// <param name="strSql">The strSql<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetCountSql(string strSql)
        {
            string countSql = string.Empty;
            string strSqlCopy = strSql.ToLower();
            int selectIndex = strSqlCopy.IndexOf("SELECT ");
            int lastFromIndex = strSqlCopy.LastIndexOf(" FROM ");
            if (selectIndex >= 0 && lastFromIndex >= 0)
            {
                int backFromIndex = strSqlCopy.LastIndexOf(" FROM ", lastFromIndex);
                int backSelectIndex = strSqlCopy.LastIndexOf("SELECT ", lastFromIndex);
                if (backSelectIndex == selectIndex)
                {
                    countSql = "SELECT COUNT(*) " + strSql.Substring(lastFromIndex);
                    return countSql;
                }
            }
            countSql = "SELECT COUNT(1) FROM (" + strSql + ") t";
            return countSql.ToLower();
        }
    }
}
