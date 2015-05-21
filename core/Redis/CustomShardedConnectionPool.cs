using ServiceStack.Redis;

namespace LComplete.Framework.Redis
{
   /// <summary>
    /// 提供可进行分布式的redis连接池（自定义后使得可以提供一些配置）
    /// </summary>
    public class CustomShardedConnectionPool : PooledRedisClientManager
    {
        /// <summary>
        /// logical name
        /// </summary>
        public readonly string name;

        /// <summary>
        /// An arbitrary weight relative to other nodes
        /// </summary>
        public readonly int weight;


       /// <param name="name">logical name</param>
       /// <param name="weight">An arbitrary weight relative to other nodes</param>
       /// <param name="initalDb">初始db</param>
       /// <param name="config">redis客户端配置</param>
       /// <param name="readWriteHosts">redis nodes</param>
       public CustomShardedConnectionPool(string name, int weight,RedisClientManagerConfig config,long initalDb, params string[] readWriteHosts)
            : base(readWriteHosts,readWriteHosts,config,initalDb,null,null)
        {
            this.name = name;
            this.weight = weight;
        }

        public override int GetHashCode()
        {
            // generate hashcode based on logial name
            // server alias/ip can change without 
            // affecting the consistent hash
            return name.GetHashCode();
        }

       public override string ToString()
       {
           return name;
       }
    }
}
