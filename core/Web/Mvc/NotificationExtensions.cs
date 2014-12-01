using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using LComplete.Framework.Web.Mvc;

namespace LComplete.Framework.Web.Extensions
{
    public static class NotificationExtensions
    {
        public static IHtmlString ShowNotice(this HtmlHelper htmlHelper, NotificationMessage notificationMessage = null,
                                             string viewName = "_Notice")
        {
            IList<NotificationMessage> messages = new List<NotificationMessage>();
            if (notificationMessage == null)
            {
                Array arrNoticeType = Enum.GetValues(typeof(NotificationType));
                var tempData = htmlHelper.ViewContext.TempData;
                foreach (NotificationType notificationType in arrNoticeType)
                {
                    if (tempData.ContainsKey(notificationType.ToString()))
                    {
                        messages.Add(new NotificationMessage(notificationType,
                                                             tempData[notificationType.ToString()].ToString()));
                    }
                }
            }
            else
            {
                messages.Add(notificationMessage);
            }
            return htmlHelper.Partial(viewName, messages);
        }
    }
}
