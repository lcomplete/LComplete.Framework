using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using NewProject.Site.Code;
using IActionFilter = System.Web.Mvc.IActionFilter;

namespace NewProject.Site.Filters
{
    /// <summary>
    /// CDN域名过滤特性
    /// </summary>
    public class CdnDomainFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 使用CDN域名访问页面时返回404
            if (filterContext.RequestContext.HttpContext.Request.Url.Host.Equals(AppConfig.CdnDomain,
                StringComparison.OrdinalIgnoreCase))
            {
                filterContext.RequestContext.HttpContext.Response.StatusCode = 404;
                filterContext.RequestContext.HttpContext.Response.End();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}