using System.Web.Mvc;
using System.Web.Security;
using Domain.Model;
using LComplete.Framework.IoC;
using LComplete.Framework.Web.Common;
using LComplete.Framework.Web.Mvc;
using Services;

namespace Admin
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

        private Auth_User _auth_User;

        public Auth_User Auth_User
        {
            get
            {
                if (_auth_User == null && IsAuthenticated)
                {
                    var userService = ContainerManager.Resolve<IAuth_UserService>();
                    _auth_User = userService.GetUser(UserId);

                    //由于测试环境与正式环境的用户不同 可能导致验证通过却无法获取用户信息 若出现这种情况 则登出系统
                    if (_auth_User == null)
                    {
                        FormsAuthentication.SignOut();
                        Response.Redirect(Url.Action("Index", "Login"));
                    }
                }

                return _auth_User;
            }
        }

        protected void Notice(NotificationType notificationType,string message)
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

        protected void FormErrorNotice()
        {
            Notice(NotificationType.Error, "表单提交失败，请按提示重新填写。");
        }
    }
}