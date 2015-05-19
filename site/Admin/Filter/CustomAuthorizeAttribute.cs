using System.Net;
using System.Web.Mvc;
using Admin.Controllers;
using Admin.Extensions;

namespace Admin.Filter
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.Controller is LoginController)
                return;
            if (filterContext.Controller is LogoutController)
                return;

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.SetPermissionData(filterContext);
                if (filterContext.Controller.GetCurrentUser() == null)
                {
                    filterContext.Result = new RedirectResult("~/Logout");
                    return;
                }

                if (!filterContext.Controller.HasPermission())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    filterContext.Result = new ViewResult() { ViewName = "~/Views/Error/Forbidden.cshtml" };
                    return;
                }
            }

            base.OnAuthorization(filterContext);
        }

    }
}