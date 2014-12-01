using System;
using System.Web;

namespace LComplete.Framework.Common
{
    public static class UrlUtils
    {
        /// <summary>
        /// 是否是域名
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public static bool IsDomain(string siteUrl)
        {
            try
            {
                if (!siteUrl.StartsWith("http://") && !siteUrl.StartsWith("https://"))
                {
                    siteUrl = "http://" + siteUrl;
                }
                Uri uri = new Uri(siteUrl);
                return uri.PathAndQuery=="/";
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取域名部分
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomain(string url)
        {
            try
            {
                url = url.ToLower();
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "http://" + url;
                }
                Uri uri = new Uri(url);
                return uri.Host;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// url编码使用大写字母
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UpperCaseUrlEncode(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp);
        }

    }
}
