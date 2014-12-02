using System.Web.Mvc;
using LComplete.Framework.Site.Domain.Model;
using LComplete.Framework.Site.Services;

namespace LComplete.Framework.Site.Web.UI.Controllers
{
    public class UserController : SiteBaseController
    {
        private IUserInfoService _userInfoService;

        public UserController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        public ActionResult Index()
        {
            UserInfo user = _userInfoService.GetUserInfo(1);
            InfoNotice(user.Password);
            ViewBag.Username = user.Username;
            ViewBag.CurrentTime = _userInfoService.GetCurrentTime();
            return View();
        }

    }
}
