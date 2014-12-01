using System.Web;
using System.Web.Mvc;
using LComplete.Framework.DependencyResolution;
using LComplete.Framework.Web.Mvc;

namespace LComplete.Framework.Web
{
    public class BaseHttpApplication:HttpApplication
    {
        static BaseHttpApplication()
        {
            #region 依赖解析器

            ContainerFactory.Singleton.Init();

            IDependencyResolver resolver = new ContainerDependencyResolver(ContainerFactory.Singleton);
            DependencyResolver.SetResolver(resolver);

            #endregion
        }
    }
}
