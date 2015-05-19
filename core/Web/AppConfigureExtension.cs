using System.Web.Mvc;
using LComplete.Framework.IoC;
using LComplete.Framework.Web.Mvc;

namespace LComplete.Framework.Web
{
    public static class AppConfigureExtension
    {
        public static BootConfigure MvcUseContainerResolver(this BootConfigure configure)
        {
            IDependencyResolver resolver = new ContainerDependencyResolver(ContainerManager.GetContainer());
            DependencyResolver.SetResolver(resolver);

            return configure;
        }
    }
}