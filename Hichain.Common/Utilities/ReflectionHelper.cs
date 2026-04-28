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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hichain.Common.Utilities
{
    /// <summary>
    /// Defines the <see cref="ReflectionHelper" />.
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// Defines the dictCache.
        /// </summary>
        private static ConcurrentDictionary<string, object> dictCache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 得到类里面的属性集合.
        /// </summary>
        /// <param name="type">.</param>
        /// <param name="columns">.</param>
        /// <returns>.</returns>
        public static PropertyInfo[] GetProperties(Type type, string[] columns = null)
        {
            PropertyInfo[] properties = null;
            if (dictCache.ContainsKey(type.FullName))
            {
                properties = dictCache[type.FullName] as PropertyInfo[];
            }
            else
            {
                properties = type.GetProperties();
                dictCache.TryAdd(type.FullName, properties);
            }
            if (columns != null && columns.Length > 0)
            {
                //  按columns顺序返回属性
                var columnPropertyList = new List<PropertyInfo>();
                foreach (var column in columns)
                {
                    var columnProperty = properties.Where(p => p.Name == column).FirstOrDefault();
                    if (columnProperty != null)
                    {
                        columnPropertyList.Add(columnProperty);
                    }
                }
                return columnPropertyList.ToArray();
            }
            else
            {
                return properties;
            }
        }
    }
}
