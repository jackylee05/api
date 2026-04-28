///*********************************************************************
///Copyright (C) 2016-2019 56-cloud.com 
///Project Name          : 56SCM 
///Create By             : hkwong.wang@qq.com
///Create Date           : 2018-11-11
///Last Updated By       :
///Last Updated Date     :
///Version               :2018-11-11
///*********************************************************************


namespace Hichain.Common.Models
{
    /// <summary>
    /// 分页参数.
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pagination"/> class.
        /// </summary>
        public Pagination()
        {
            Sort = "Id"; // 默认按Id排序
            SortType = " desc ";
            PageIndex = 1;
            PageSize = 20;
        }

        /// <summary>
        /// Gets or sets the PageSize
        /// 每页行数.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the PageIndex
        /// 当前页.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the Sort
        /// 排序列.
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Gets or sets the SortType
        /// 排序类型.
        /// </summary>
        public string SortType { get; set; }

        /// <summary>
        /// Gets or sets the TotalCount
        /// 总记录数.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets the TotalPage
        /// 总页数.
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (TotalCount > 0)
                {
                    return TotalCount % this.PageSize == 0 ? TotalCount / this.PageSize : TotalCount / this.PageSize + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
