using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Admin.Areas.Auth.Models;
using Admin.Models;
using Domain.Exception;
using Domain.InfoModel;
using Domain.Model;
using LComplete.Framework.Exceptions;
using LComplete.Framework.Web.Mvc;
using Services;

namespace Admin.Areas.Auth.Controllers
{
    public class UserController : BaseController
    {
        private IAuth_UserService _authUserService;
        private IAuth_GroupService _authGroupService;

        public UserController(IAuth_UserService authUserService, IAuth_GroupService authGroupService)
        {
            _authUserService = authUserService;
            _authGroupService = authGroupService;
        }

        public ActionResult Index(string username)
        {
            IList<Auth_User> users = _authUserService.GetUsers(username);
            return View(users);
        }

        public ActionResult Edit(int id)
        {
            if (id > 0)
            {
                Auth_User user = _authUserService.GetUser(id, t => t.Auth_User_Groups);
                if (user != null)
                {
                    IList<Auth_Group> authGroups = _authGroupService.GetGroups();

                    EditUserViewModel viewModel=AutoMapper.Mapper.Map<EditUserViewModel>(user);
                    viewModel.Auth_Groups = authGroups;
                    viewModel.SelectGroupIds = user.Auth_User_Groups.Select(t => t.GroupId).ToList();

                    return View(viewModel);
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                Auth_UserProfile userProfile = AutoMapper.Mapper.Map<Auth_UserProfile>(userViewModel);
                try
                {
                    _authUserService.UpdateProfile(userProfile);
                    Notice(NotificationType.Success, string.Format("用户 {0} 修改成功", userProfile.Username));

                    return RedirectToAction("Index");
                }
                catch (DataAlreadyExistsException dre)
                {
                    Notice(NotificationType.Error, dre.Message);
                }
                catch(UnAcceptOperationException uoe)
                {
                    ErrorNotice(uoe.Message);
                }
            }
            else
            {
                FormErrorNotice();
            }

            userViewModel.Auth_Groups = _authGroupService.GetGroups();
            return View(userViewModel);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(NewUserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Auth_User user = _authUserService.AddNewUser(newUser.Username, newUser.Password);
                    return RedirectToAction("Edit", new {id = user.Id});
                }
                catch(DataAlreadyExistsException dre)
                {
                    ErrorNotice(dre.Message);
                }
            }
            else
            {
                FormErrorNotice();
            }

            return View(newUser);
        }

        [HttpPost]
        public ActionResult ChangePassword(int id,PasswordViewModel passwordView)
        {
            if(ModelState.IsValid)
            {
                _authUserService.ChangePassword(id, passwordView.Password);
                Notice(NotificationType.Success, "密码修改成功");
            }
            else
                Notice(NotificationType.Error, "密码验证失败，请重新输入");

            return RedirectToAction("Edit", new { id = id }); 
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id == UserId)
            {
                ErrorNotice("不能删除自己！");
            }
            else
            {
                try
                {
                    _authUserService.DeleteUser(id);
                    SuccessNotice("成功删除了一位用户");
                }
                catch (UnAcceptOperationException uoe)
                {
                    ErrorNotice(uoe.Message);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
