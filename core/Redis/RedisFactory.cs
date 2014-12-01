using LComplete.Framework.Cache;
using ServiceStack.Redis;

namespace LComplete.Framework.Redis
{
    /// <summary>
    /// 功能：Redis客户端工厂
    /// 作者：娄晨
    /// 日期：2014-5-20
    /// </summary>
    public static class RedisFactory
    {
        /// <summary>
        /// Redis客户端连接分区管理对象 （利用一致性哈希算法提供客户端连接分区）
        /// </summary>
        private static readonly AutoDetectShardedRedisClientManager RedisClientManager;

        static RedisFactory()
        {
            CacheSetting cacheSetting = CacheSetting.GetInstance();
            if (cacheSetting != null)
            {
                var connectionPools = new CustomShardedConnectionPool[cacheSetting.Pools.Count];
                int index = 0;
                foreach (CacheSetting.PoolConfig poolConfig in cacheSetting.Pools)
                {
                    long initalDb = poolConfig.Db ?? 0L;
                    RedisClientManagerConfig config = poolConfig.MaxReadPoolSize.HasValue || poolConfig.MaxWritePoolSize.HasValue?new RedisClientManagerConfig():null;
                    if (config != null)
                    {
                        config.MaxReadPoolSize = poolConfig.MaxReadPoolSize.HasValue
                            ? poolConfig.MaxReadPoolSize.Value
                            : 50;
                        config.MaxWritePoolSize = poolConfig.MaxWritePoolSize.HasValue
                            ? poolConfig.MaxWritePoolSize.Value
                            : 50;
                    }
                    var pool = new CustomShardedConnectionPool(poolConfig.Name, poolConfig.Weight,config,initalDb,
                        poolConfig.Hosts);
                    pool.PoolTimeout = poolConfig.PoolTimeout ?? 1000;
                    pool.ConnectTimeout = poolConfig.ConnectTimeout;
                    pool.SocketSendTimeout = poolConfig.SocketSendTimeout;
                    pool.SocketReceiveTimeout = poolConfig.SocketReceiveTimeout;

                    connectionPools[index] = pool;
                    index++;
                }
                RedisClientManager = new AutoDetectShardedRedisClientManager(connectionPools);
            }
        }

        /// <summary>
        /// 通过Key映射得到RedisClient对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IRedisClient CreateClient(string key)
        {
            if (RedisClientManager == null)
                return null;

            CustomShardedConnectionPool pool = RedisClientManager.GetConnectionPool(key); //通过key映射到指定的连接池
            return pool != null ? pool.GetClient() : null;
        }
    }
}