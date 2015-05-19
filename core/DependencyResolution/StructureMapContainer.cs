using System;
using System.Collections.Generic;
using LComplete.Framework.IoC;

namespace LComplete.Framework.DependencyResolution
{
    internal class StructureMapContainer : IContainer
    {
        private StructureMap.IContainer _mapContainer;

        public StructureMapContainer(StructureMap.IContainer mapContainer)
        {
            _mapContainer = mapContainer;
        }

        public T Resolve<T>()
        {
            return _mapContainer.GetInstance<T>();
        }

        public object Resolve(Type modelType)
        {
            return _mapContainer.GetInstance(modelType);
        }

        public IList<object> ResolveAll(Type modelType)
        {
            return (IList<object>)_mapContainer.GetAllInstances(modelType);
        }

        public object TryResolve(Type modelType)
        {
            return _mapContainer.TryGetInstance(modelType);
        }

        public void Register(Type typeFor, Type typeUse)
        {
            _mapContainer.Configure(x => x.For(typeFor).Use(typeUse));
        }

        public void Register(Type typeFor, object instance)
        {
            _mapContainer.Configure(x => x.For(typeFor).Use(instance));
        }

        public void Register<TAbstract>(TAbstract singletonOfConcrete) where TAbstract : class
        {
            _mapContainer.Configure(x => x.For<TAbstract>().Use(singletonOfConcrete));
        }

        public void Register<TAbstract, TConcrete>(bool singleton = false) 
            where TAbstract : class where TConcrete : class, TAbstract
        {
            _mapContainer.Configure(x =>
            {
                var binding= x.For<TAbstract>().Use<TConcrete>();
                if (singleton)
                {
                    binding.Singleton();
                }
                else
                {
                    binding.AlwaysUnique();
                }
            });
        }

    }

}
