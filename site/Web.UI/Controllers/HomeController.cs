using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LComplete.Framework.Site.Domain.QueryCondition;
using LComplete.Framework.Site.Services;

namespace LComplete.Framework.Site.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private ICustomService _service;

        public HomeController(ICustomService service)
        {
            this._service = service;
        }

        public ActionResult Index(CustomOrderQuery orderQuery)
        {
            var list= _service.GetCustomList(orderQuery);
            return View(list);
        }

    }
}
