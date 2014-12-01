using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LComplete.Framework.Data
{
    public class PagingQuery
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 每一页的数量 默认为20页
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 是否获取总数
        /// </summary>
        public bool IsGetRecordCount { get; set; }

        public PagingQuery()
        {
            Page = 1;
            PageSize = 20;
            IsGetRecordCount = true;
        }
    }

    public static class PagingQueryExtensions
    {
        public static int GetSkipCount(this PagingQuery query)
        {
            return (query.Page - 1) * query.PageSize;
        }
    }
}
