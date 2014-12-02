using System;
using System.Web;
using System.Web.Security;

namespace LComplete.Framework.Web.Common
{
    public static class AuthenticationUtils
    {
        /// <summary>
        /// 设置登陆cookie
        /// </summary>
        /// <param name="username"></param>
        /// <param name="createPersistentCookie"></param>
        /// <param name="userId"></param>
        public static void SetAuthCookie(string username, bool createPersistentCookie, int userId)
        {
            var ticket = new FormsAuthenticationTicket(1, username, DateTime.Now,
                DateTime.Now.AddMinutes(
                    FormsAuthentication.Timeout.TotalMinutes),
                createPersistentCookie, userId.ToString());
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            cookie.Value = FormsAuthentication.Encrypt(ticket);
            cookie.HttpOnly = true;
            cookie.Domain = FormsAuthentication.CookieDomain;

            if (ticket.IsPersistent)
                cookie.Expires = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取登录用户Id 无法获取时返回0
        /// </summary>
        /// <returns></returns>
        public static int GetLoginUserId()
        {
            if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
                if (formsIdentity != null)
                    return int.Parse(formsIdentity.Ticket.UserData);
            }

            return 0;
        }
    }
}
