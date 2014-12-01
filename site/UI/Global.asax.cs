using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using LComplete.Framework.Log4Net;
using LComplete.Framework.Logging;
using LComplete.Framework.Web;

namespace LComplete.Framework.Site.UI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : BaseHttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            LogManager.LogFactory=new Log4NetFactory("log4net.config");
        }
    }
}