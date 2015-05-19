using System;
using System.Security.Cryptography;

namespace LComplete.Framework.Common
{
    public static class RandomUtils
    {
        public static string GetRandomFileName()
        {
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash, 4, 8).Replace("-", "").ToLower();
        }
    }
}