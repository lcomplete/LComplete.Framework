using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LComplete.Framework.DependencyResolution;

namespace LComplete.Framework.Web.Mvc
{
    public class ContainerDependencyResolver : IDependencyResolver
    {
        private IObjectContainer _objectContainer;

        public ContainerDependencyResolver(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsSubclassOf(typeof(Controller)))
            {
                return _objectContainer.Resolve(serviceType);
            }

            object result = _objectContainer.TryResolve(serviceType);
            return result;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _objectContainer.ResolveAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}
