using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LComplete.Framework.CacheHandler;
using LComplete.Framework.Log4Net;
using LComplete.Framework.Logging;
using LComplete.Framework.Web;

namespace LComplete.Framework.Site.Web.UI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : BaseHttpApplication
    {
        protected override void Application_Start()
        {
            LogManager.LogFactory = new Log4NetFactory(Server.MapPath("log4net.config"));

            base.Application_Start();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // 配置缓存拦截器
            if (ConfigurationManager.AppSettings["CacheAop_Assembles"] != null)
                CachingAopConfig.Config(ConfigurationManager.AppSettings["CacheAop_Assembles"]);
        }
    }
}