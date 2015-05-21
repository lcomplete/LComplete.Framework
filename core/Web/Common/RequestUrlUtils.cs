using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace LComplete.Framework.Web.Common
{
    public static class RequestUrlUtils
    {
        /// <summary>
        /// 按指定参数和值重组当前请求的Url
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paramKey"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static string CombinationRequestUrl(RequestContext context, string paramKey, string paramValue)
        {
            HttpRequestBase req = context.HttpContext.Request;
            string url = req.FilePath;
            var queryStrings = new NameValueCollection(req.QueryString);

            if (queryStrings.AllKeys.Contains(paramKey))
            {
                queryStrings[paramKey] = paramValue;
            }
            else
                queryStrings.Add(paramKey, paramValue);

            var qs = new string[queryStrings.Count];
            for (int i = 0; i < queryStrings.Count; i++)
            {
                qs[i] = queryStrings.Keys[i] + "=" + queryStrings[i];
            }

            return string.Concat(new[] {url, "?", string.Join("&", qs)});
        }
    }
}
