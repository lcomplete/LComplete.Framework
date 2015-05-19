using System.Web.Mvc;
using NewProject.Site.Filters;

namespace LComplete.Framework.Site.Web.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CdnDomainFilterAttribute());
        }
    }
}