using System;
using System.Web;
using LComplete.Framework.Data.CommonQuery;

namespace Admin.Extensions
{
    public static class QueryExtensions
    {
        public static void SetDateRange(this IDateRangeQuery query)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.QueryString["StartDate"] == null && request.QueryString["EndDate"] == null)
            {
                query.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                query.EndDate = DateTime.Now.Date;
            }
            else if(query.StartDate.HasValue && query.EndDate.HasValue && query.StartDate>query.EndDate)
            {
                DateTime? tempDate = query.StartDate;
                query.StartDate = query.EndDate;
                query.EndDate = tempDate;
            }
        }
    }
}