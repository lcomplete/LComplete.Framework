using System;

namespace LComplete.Framework.Cache
{
    public interface ICache
    {
        bool Set(string key, object value);
        bool Set(string key, object value, DateTime expiresAt);
        bool Set(string key, object value, TimeSpan expiresIn);

        object Get(string key);
        T Get<T>(string key);

        bool Remove(string key);

        void FlushAll();
    }
}
