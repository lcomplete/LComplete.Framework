using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LComplete.Framework;
using LComplete.Framework.CacheHandler;
using LComplete.Framework.DependencyResolution;
using LComplete.Framework.Log4Net;
using LComplete.Framework.Web;
using LComplete.Framework.Web.Auth;

namespace Admin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            BootConfigure.Boot()
                .UseStructureMap("LComplete.Framework.Site.Domain", "LComplete.Framework.Site.DateAccess", "LComplete.Framework.Site.Services")
                .MvcUseContainerResolver()
                .UseLog4Net(Server.MapPath("log4net.config"));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.Configure();
            FunctionConfig.RegisterFunctions(FunctionTable.Functions);

            // 配置缓存拦截器
            if (ConfigurationManager.AppSettings["CacheAop_Assembles"] != null)
                CachingAopConfig.Config(ConfigurationManager.AppSettings["CacheAop_Assembles"]);
        }
    }
}