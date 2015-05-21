using System;

namespace LComplete.Framework.Data.CommonQuery
{
    public interface IDateRangeQuery
    {
        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }
    }
}
