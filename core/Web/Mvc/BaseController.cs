using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LComplete.Framework.Web.Common;

namespace LComplete.Framework.Web.Mvc
{
    public class BaseController:Controller
    {
        public bool IsAuthenticated
        {
            get { return User.Identity.IsAuthenticated; }
        }

        public string UserName
        {
            get { return User.Identity.Name; }
        }

        public int UserId { get { return AuthenticationUtils.GetLoginUserId(); } }

        protected void Notice(NotificationType notificationType, string message)
        {
            NotificationManager.Notice(this, notificationType, message);
        }

        protected void SuccessNotice(string message)
        {
            Notice(NotificationType.Success, message);
        }

        protected void WarningNotice(string message)
        {
            Notice(NotificationType.Warning, message);
        }

        protected void InfoNotice(string message)
        {
            Notice(NotificationType.Info, message);
        }

        protected void ErrorNotice(string message)
        {
            Notice(NotificationType.Error, message);
        }
    }
}
