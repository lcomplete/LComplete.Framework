﻿using System.Collections.Generic;

namespace LComplete.Framework.Data
{
    /// <summary>
    /// 分页数据源
    /// </summary>
    public class PagingDataSource<T> : PagingModel where T : class
    {
        /// <summary>
        /// 排序字段集合
        /// </summary>
        public OrderFieldStore<T> OrderFieldStore { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public IList<T> DataSource { get; set; }

        public PagingDataSource(IList<T> dataSource, int pageIndex, int pageSize, int recordCount = 0)
            : base(pageIndex, pageSize, recordCount)
        {
            DataSource = dataSource;
        }

        public PagingDataSource(IList<T> dataSource, PagingQuery pagingQuery, int recordCount = 0) :
            this(dataSource, pagingQuery.Page, pagingQuery.PageSize, recordCount)
        {

        }

    }
}
