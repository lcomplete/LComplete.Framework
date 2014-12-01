using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using LComplete.Framework.Cache;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace LComplete.Framework.Redis
{
    /// <summary>
    /// 混合缓存二进制和json序列化的数据
    /// </summary>
    public class HybridRedisCache:ICache
    {
        /// <summary>
        /// 将缓存值包装为强类型
        /// </summary>
        class ValueWrapper<T>
        {
            public T Value { get; private set; }

            public Type ValueType { get; set; }

            public ValueWrapper(T value)
            {
                Value = value;
                ValueType = value.GetType();
            }
        }

        /// <summary>
        /// 是否是可序列化的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsSerializableType(Type type)
        {
            if (type == typeof (Hashtable))
            {
                return false;
            }

            bool isSerializable = HasSerializableAttribute(type);
            if (type.IsGenericType)
            {
                Type[] genericArgs= type.GetGenericArguments();
                foreach (Type genericArg in genericArgs)
                {
                    isSerializable = HasSerializableAttribute(genericArg);
                    if(!isSerializable)
                        break;
                }
            }

            return isSerializable;
        }

        /// <summary>
        /// 类型上是否有可序列化属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool HasSerializableAttribute(Type type)
        {
            return type.GetCustomAttributes(typeof (SerializableAttribute), false).Count() > 0;
        }

        private static byte[] BinarySerialize(object value)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, value);
                return memoryStream.ToArray();
            }
        }

        public bool Set(string key, object value)
        {
            IRedisClient client = RedisFactory.CreateClient(key);
            if (client == null)
                return false;

            using (client)
            {
                if (IsSerializableType(value.GetType()))
                    return client.Set(key, BinarySerialize(value));

                return client.Set(key, new ValueWrapper<object>(value));
            }
        }

        public bool Set(string key, object value, DateTime expiresAt)
        {
            IRedisClient client = RedisFactory.CreateClient(key);
            if (client == null)
                return false;

            using (client)
            {
                if(IsSerializableType(value.GetType()))
                    return client.Set(key, BinarySerialize(value), expiresAt);

                return client.Set(key, new ValueWrapper<object>(value), expiresAt);
            }
        }

        public bool Set(string key, object value, TimeSpan expiresIn)
        {
            IRedisClient client = RedisFactory.CreateClient(key);
            if (client == null)
                return false;

            using (client)
            {
                if(IsSerializableType(value.GetType()))
                    return client.Set(key, BinarySerialize(value), expiresIn);
                
                return client.Set(key, new ValueWrapper<object>(value), expiresIn);
            }
        }

        public object Get(string key)
        {
            return Get<object>(key);
        }

        public T Get<T>(string key)
        {
            IRedisClient client = RedisFactory.CreateClient(key);
            if (client == null)
                return default(T);

            T result = default(T);
            using (client)
            {
                bool isDeserializable = IsSerializableType(typeof(T));
                if (isDeserializable)
                {
                    try
                    {
                        byte[] buffer = client.Get<byte[]>(key);
                        //先尝试进行二进制序列化
                        if (buffer != null)
                        {
                            using (MemoryStream ms = new MemoryStream(buffer))
                            {
                                object obj = new BinaryFormatter().Deserialize(ms);
                                result = (T) obj;
                            }
                        }
                    }
                    catch
                    {
                        isDeserializable = false;
                    }
                }

                if(!isDeserializable)
                {
                    string cacheValue = client.GetValue(key);
                    ValueWrapper<T> wrapper = JsonSerializer.DeserializeFromString<ValueWrapper<T>>(cacheValue);
                    if (wrapper != null)
                    {
                        if (wrapper.Value.GetType() == wrapper.ValueType) //判断序列化的类型与实际类型是否一致
                            result = wrapper.Value;
                        else
                        {
                            Type wrapperType = typeof (ValueWrapper<>);
                            wrapperType = wrapperType.MakeGenericType(wrapper.ValueType);
                            MethodInfo method = typeof (JsonSerializer).GetMethod("DeserializeFromString",new []{typeof(string)});
                            MethodInfo generic = method.MakeGenericMethod(wrapperType);
                            object corretResult = generic.Invoke(null, new[] {cacheValue});
                            if (corretResult != null)
                            {
                                result= (T) wrapperType.GetProperty("Value").GetValue(corretResult, null);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool Remove(string key)
        {
            IRedisClient client = RedisFactory.CreateClient(key);
            if (client == null)
                return false;

            using (client)
            {
                return client.Remove(key);
            }
        }

        public void FlushAll()
        {
            throw new NotSupportedException("缓存为分布式结构，暂不支持清空所有缓存");
        }
    }
}
