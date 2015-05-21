using System;
using System.Collections.Generic;

namespace LComplete.Framework.Cache
{
    public static class CacheFactory
    {
        private static readonly Dictionary<string, ICache> DictCacheProviders = new Dictionary<string, ICache>();

        private static readonly object SyncRoot = new object();

        public static ICache CreateDefault()
        {
            CacheSetting setting = CacheSetting.GetInstance();
            string cacheProvider = setting != null ? (setting.CacheProvider ?? string.Empty) : string.Empty;
            ICache cache = null;

            if (!DictCacheProviders.TryGetValue(cacheProvider, out cache))
            {
                lock (SyncRoot)
                {
                    if (!DictCacheProviders.TryGetValue(cacheProvider, out cache))
                    {
                        if (!string.IsNullOrEmpty(cacheProvider))
                        {
                            cache = (ICache)Activator.CreateInstance(Type.GetType(cacheProvider));
                        }
                        else
                            cache = new WebCache();
                        DictCacheProviders.Add(cacheProvider, cache);
                    }
                }
            }

            return cache;
        }
    }
}
