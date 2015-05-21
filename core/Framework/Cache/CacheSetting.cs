using System.Collections.Generic;
using LComplete.Framework.Setting;

namespace LComplete.Framework.Cache
{
    public class CacheSetting :ISetting 
    {
        public static CacheSetting GetInstance()
        {
            return CustomSetting.GetConfig<CacheSetting>();
        }

        public class PoolConfig
        {
            /// <summary>
            /// 连接池名称（必须唯一，以做hash）
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 权重（权重越高 虚拟结点越多）
            /// </summary>
            public int Weight { get; set; }

            /// <summary>
            /// redis 地址
            /// </summary>
            public string Hosts { get; set; }

            /// <summary>
            /// db实例（redis默认支持16个db实例，每个实例之间的数据具有隔离性，默认为0）
            /// </summary>
            public long? Db { get; set; }

            /// <summary>
            /// 获取连接池超时时间（默认1000毫秒）
            /// </summary>
            public int? PoolTimeout { get; set; }

            /// <summary>
            /// 连接redis超时时间
            /// </summary>
            public int? ConnectTimeout { get; set; }

            /// <summary>
            /// socket发送超时时间
            /// </summary>
            public int? SocketSendTimeout { get; set; }

            /// <summary>
            /// socket 接收超时时间
            /// </summary>
            public int? SocketReceiveTimeout { get; set; }

            /// <summary>
            /// 最大只读连接池大小（默认为50）
            /// </summary>
            public int? MaxReadPoolSize { get; set; }

            /// <summary>
            /// 最大读写连接池大小（默认为50）
            /// </summary>
            public int? MaxWritePoolSize { get; set; }
        }

        public List<PoolConfig> Pools { get; set; }

        public string CacheProvider { get; set; }

        public string CachePrefix { get; set; }
    }

    
}
