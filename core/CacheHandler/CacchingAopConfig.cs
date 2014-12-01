using Snap;
using Snap.StructureMap;

namespace LComplete.Framework.CacheHandler
{

    public static class CacchingAopConfig
    {
        public static void Config(string includeNamespace)
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace(includeNamespace);
                c.Bind<CachingInterceptor>().To<CachingAttribute>();
            });
        }

    }

}
