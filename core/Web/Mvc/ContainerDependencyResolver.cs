using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LComplete.Framework.IoC;

namespace LComplete.Framework.Web.Mvc
{
    public class ContainerDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public ContainerDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsSubclassOf(typeof(Controller)))
            {
                return _container.Resolve(serviceType);
            }

            object result = _container.TryResolve(serviceType);
            return result;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}
