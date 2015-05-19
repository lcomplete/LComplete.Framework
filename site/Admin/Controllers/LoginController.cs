using System;
using System.Web.Mvc;
using Admin.Models;
using Domain.Model;
using LComplete.Framework.Web.Common;
using Services;

namespace Admin.Controllers
{
    public class LoginController : BaseController
    {
        private IAuth_UserService _userService;

        public LoginController(IAuth_UserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginModel,string returnUrl)
        {
            if(ModelState.IsValid)
            {
                Auth_User user = _userService.ValidateUser(loginModel.Username,loginModel.Password);
                if(user!=null)
                {
                    AuthenticationUtils.SetAuthCookie(user.Username,loginModel.IsRememberMe,user.Id);
                    _userService.UpdateLoginDate(user.Id, DateTime.Now);
                    TempData["JustLogin"] = true;

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ErrorNotice("用户名或密码填写错误");
                }
            }
            else
            {
                ErrorNotice("请填写用户名和密码");
            }

            return View(loginModel);
        }

    }
}
