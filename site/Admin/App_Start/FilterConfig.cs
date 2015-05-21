using System.Web.Mvc;
using Admin.Filter;

namespace Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ErrorLogAttribute());
            filters.Add(new CustomAuthorizeAttribute());
        }
    }
}