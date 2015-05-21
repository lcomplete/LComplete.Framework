using System.Net;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Forbidden()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View();
        }

    }
}
