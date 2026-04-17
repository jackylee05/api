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

namespace Hichain.Common.Utilities
{
    /// <summary>
    /// Defines the <see cref="Extensions" />.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// The GetOriginalException.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        /// <returns>The <see cref="Exception"/>.</returns>
        public static Exception GetOriginalException(this Exception ex)
        {
            if (ex.InnerException == null) return ex;
            return ex.InnerException.GetOriginalException();
        }
    }
}
