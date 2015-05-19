using System.IO;
using System.Web;
using LComplete.Framework.Common;

namespace LComplete.Framework.Web.Common
{
    public static class UrlUtils
    {
        /// <summary>
        /// 在文件路径后自动添加版本号(最后修改时间)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string AutoVersion(string filePath)
        {
            string physicPath= HttpContext.Current.Server.MapPath(filePath);
            if (File.Exists(physicPath))
            {
                FileInfo fi = new FileInfo(physicPath);
                return filePath + "?" + fi.LastWriteTime.ToTimeStamp();
            }

            return filePath;
        }
    }
}