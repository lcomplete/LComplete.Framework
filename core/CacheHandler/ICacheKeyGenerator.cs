using System.Reflection;

namespace LComplete.Framework.CacheHandler
{
    interface ICacheKeyGenerator
    {
        string CreateCacheKey(MethodBase method, object[] inputs);
    }
}
