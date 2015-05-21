using System.IO;
using WcfContract.Messages;

namespace FileServer.Code
{
    public class PathUtils
    {
        public static string GetThumbPath(string relativePath, int width, int? height)
        {
            string path = "thumb\\w" + width + (height.HasValue ? "_" + height : "") + "\\" + relativePath;
            return GetFilePath(path);
        }

        public static string GetFilePath(string relativePath)
        {
            return Path.Combine(AppConfig.UploadSavePath, relativePath);
        }
    }
}