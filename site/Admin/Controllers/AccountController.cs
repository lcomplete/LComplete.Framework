using System.Web.Mvc;
using Admin.Models;
using Domain.Model;
using Services;

namespace Admin.Controllers
{
    public class AccountController : BaseController
    {
        private IAuth_UserService _userService;

        public AccountController(IAuth_UserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View(_userService.GetUser(UserId, t => t.Auth_User_Groups));
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel passwordView)
        {
            if(ModelState.IsValid)
            {
                Auth_User user = _userService.ValidateUser(UserName, passwordView.RawPassword);
                if(user!=null)
                {
                    _userService.ChangePassword(user.Id, passwordView.Password);
                    SuccessNotice("密码修改成功");
                }
                else
                {
                    ErrorNotice("原密码填写错误");
                }
            }
            else
            {
                FormErrorNotice();
            }

            return View();
        }
    }
}
