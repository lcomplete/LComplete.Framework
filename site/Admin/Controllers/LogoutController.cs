using System.Web.Mvc;
using System.Web.Security;

namespace Admin.Controllers
{
    public class LogoutController : BaseController
    {
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}