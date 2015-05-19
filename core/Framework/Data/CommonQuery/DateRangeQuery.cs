using System;

namespace LComplete.Framework.Data.CommonQuery
{
    public class DateRangeQuery : PagingQuery, IDateRangeQuery
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}