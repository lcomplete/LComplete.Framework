using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using LComplete.Framework.Common;

namespace LComplete.Framework.Http
{
    /// <summary>
    /// 网页标签内容提取公用类
    /// </summary>
    public static class WebPageUtils
    {
        /// <summary>
        /// 获取网站Title
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPageTitle(string url)
        {
            try
            {
                string siteHtml = GetPageHtml(url);
                return GetTitleFromHtml(siteHtml);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetTitleFromHtml(string html)
        {
            if (html == null)
                return string.Empty;

            string title = string.Empty;
            Regex regex = new Regex("<title>(?<t>(.|\n)*?)</title>", RegexOptions.IgnoreCase);
            Match match = regex.Match(html);
            if (match.Success)
                title = match.Groups["t"].Value.Trim();
            return title;
        }

        /// <summary>
        /// 获取网站的Meta(Keywords)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPageMetaKeywords(string url)
        {
            try
            {
                var siteHtml = GetPageHtml(url);
                return GetKeywordFromHtml(siteHtml);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetKeywordFromHtml(string html)
        {
            if (html == null)
                return string.Empty;

            const string re = "(?<=meta name=\"keywords\" content=\").*?(?=\")";
            Regex regex = new Regex(re, RegexOptions.IgnoreCase);

            Match match = regex.Match(html);
            if (match.Success)
                return match.Value;

            return string.Empty;
        }

        /// <summary>
        /// 获取网站的Meta(Description)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPageMetaDescription(string url)
        {
            try
            {
                string siteHtml = GetPageHtml(url);
                return GetDescriptionFromHtml(siteHtml);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetDescriptionFromHtml(string html)
        {
            if (html == null)
                return string.Empty;

            const string re = "(?<=meta name=\"description\" content=\").*?(?=\")";
            Regex regex = new Regex(re, RegexOptions.IgnoreCase);

            Match match = regex.Match(html);
            if (match.Success)
                return match.Value;

            return string.Empty;
        }

        /// <summary>
        /// 获取页面html
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <returns></returns>
        public static string GetPageHtml(string url, string encode = null, CookieContainer cookieContainer = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Proxy = null;
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            if (cookieContainer != null && cookieContainer.Count > 0)
                webRequest.CookieContainer = cookieContainer;
            //模拟浏览器请求
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            using (Stream stream = webResponse.GetResponseStream())
            {
                if (string.IsNullOrEmpty(encode)) // 未指定编码时 推测编码
                {
                    Encoding encoding = null;
                    try
                    {
                        encoding = Encoding.GetEncoding(webResponse.CharacterSet); //从输出流中获取的编码
                    }
                    catch (Exception)
                    {
                        encoding = Encoding.UTF8;
                    }

                    byte[] buffer = StreamUtils.ReadToEnd(stream);
                    string html = encoding.GetString(buffer);
                    if (encoding.BodyName == "utf-8" || encoding.BodyName == "gb2312") //UTF-8编码优先
                        return html;
                    Encoding htmlEncoding = GetEncoding(html); //从html页面中获取编码

                    if (htmlEncoding == null || Equals(htmlEncoding, encoding))
                        return html;

                    return htmlEncoding.GetString(buffer);
                }

                using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(encode)))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static Encoding GetEncoding(string html)
        {
            string pattern = "(?i)\\bcharset=\\\"?(?<charset>[-a-zA-Z_0-9]+)\\\"?";
            string charset = Regex.Match(html, pattern).Groups["charset"].Value;
            try
            {
                return Encoding.GetEncoding(charset);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
