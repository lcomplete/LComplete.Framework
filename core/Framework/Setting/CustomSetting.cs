using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using LComplete.Framework.Common;

namespace LComplete.Framework.Setting
{
    /// <summary>
    /// 分类配置
    /// </summary>
    public class CustomSetting
    {
        private static readonly Dictionary<Type, object> _syncFileRoots = new Dictionary<Type, object>();

        private static readonly object SyncRoot = new object();

        public static readonly string ConfigPath = "custom_configs";

        private static object GetLock(Type t)
        {
            lock (SyncRoot)
            {
                if (!_syncFileRoots.ContainsKey(t))
                    _syncFileRoots.Add(t, new object());

                return _syncFileRoots[t];
            }
        }

        public static T GetConfig<T>() where T : class ,ISetting
        {
            string filePath = GetFilePath(typeof(T));
            T setting = null;

            if (File.Exists(filePath))
            {
                string cacheKey = "common_classify_setting_" + typeof(T).Name;
                setting = HttpRuntime.Cache.Get(cacheKey) as T;

                if (setting == null)
                {
                    lock (GetLock(typeof(T)))
                    {
                        setting = XmlSerializerUtils.Deserialize<T>(filePath);
                        if (setting != null)
                        {
                            HttpRuntime.Cache.Insert(cacheKey, setting, new CacheDependency(filePath));
                        }
                    }
                }
            }

            return setting;
        }

        public static void SaveConfig(ISetting setting)
        {
            if(setting==null)
                return;

            string filePath = GetFilePath(setting.GetType());
            lock (GetLock(setting.GetType()))
            {
                string dir = GetFileDir();
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                XmlSerializerUtils.Serializer(filePath, setting);
            }
        }

        private static string GetFilePath(Type type)
        {
            return GetFileDir() + type.Name + ".setting";
        }

        private static string GetFileDir()
        {
            string dir = HttpContext.Current != null
                              ? HttpContext.Current.Server.MapPath("~/" + ConfigPath + "/")
                              : AppDomain.CurrentDomain.BaseDirectory + "/" + ConfigPath + "/";
            return dir;
        }
    }
}
