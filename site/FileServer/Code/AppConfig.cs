using System.Configuration;

namespace FileServer.Code
{
    public static class AppConfig
    {
        public static string UploadSavePath
        {
            get { return ConfigurationManager.AppSettings["UploadSavePath"] ?? @"D:\temp\"; }
        }
    }
}