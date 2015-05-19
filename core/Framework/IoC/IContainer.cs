using System;
using System.Collections.Generic;

namespace LComplete.Framework.IoC
{
    public interface IContainer
    {
        T Resolve<T>();

        object Resolve(Type modelType);

        object TryResolve(Type modelType);

        IList<object> ResolveAll(Type modelType);

        void Register(Type typeFor, Type typeUse);

        void Register(Type typeFor, object instance);

        void Register<TAbstract>(TAbstract singletonOfConcrete) where TAbstract : class;

        void Register<TAbstract, TConcrete>(bool singleton = false)
            where TConcrete : class, TAbstract
            where TAbstract : class;

    }
}
