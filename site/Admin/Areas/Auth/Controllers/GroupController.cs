using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Exception;
using Domain.Model;
using LComplete.Framework.Common;
using LComplete.Framework.Exceptions;
using Services;

namespace Admin.Areas.Auth.Controllers
{
    public class GroupController : BaseController
    {
        private IAuth_GroupService _authGroupService;

        public GroupController(IAuth_GroupService authGroupService)
        {
            _authGroupService = authGroupService;
        }

        public ActionResult Index()
        {
            IList<Auth_Group> groups = _authGroupService.GetGroups();
            return View(groups);
        }

        public ActionResult Edit(string id)
        {
            int groupId = ValueConverter.Parse<int>(id);
            Auth_Group group = null;
            if (groupId > 0)
            {
                group = _authGroupService.GetGroup(groupId);
            }
            if (group == null && !string.IsNullOrEmpty(id))
                return HttpNotFound();

            return View(group??new Auth_Group());
        }

        [HttpPost]
        public ActionResult Edit(Auth_Group group)
        {
            if (ModelState.IsValid)
            {
                string permissions = Request.Form["Permissions"];
                if (!string.IsNullOrEmpty(permissions))
                {
                    string[] arrPermission = permissions.Split(',');
                    foreach (var permission in arrPermission)
                    {
                        group.Auth_Group_Permissions.Add(new Auth_Group_Permissions()
                                                             {
                                                                 CreateDate = DateTime.Now,
                                                                 GroupId = group.Id,
                                                                 PermissionKey = permission
                                                             });
                    }
                }
                try
                {
                    bool isEdit = group.Id > 0;
                    _authGroupService.Save(group);
                    SuccessNotice(isEdit?string.Format("分组 {0} 修改成功", group.Name):"成功添加了一个分组");
                    return RedirectToAction("Index");
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

            return View(group);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            int result= _authGroupService.Delete(id);
            if(result==1)
            {
                SuccessNotice("成功删除了一个分组");
            }
            else if(result==-1)
            {
                ErrorNotice("分组下包含用户，无法进行删除操作");
            }

            return RedirectToAction("Index");
        }
    }
}
