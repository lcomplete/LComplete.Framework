using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Admin.Areas.Auth.Controllers;
using Admin.Controllers;
using LComplete.Framework.Web.Auth;

namespace Admin
{
    public class FunctionConfig
    {
        private static FunctionComponent fc<T>(string groupName, Expression<Func<T, object>> actionExpression,
                                               params FunctionComponent[] childFunctions) where T : Controller
        {
            return fc<T>(groupName, actionExpression, true, childFunctions);
        }

        private static FunctionComponent fc<T>(string groupName, Expression<Func<T, object>> actionExpression,bool hasMenu,
                                               params FunctionComponent[] childFunctions) where T : Controller
        {
            FunctionComponent component = new FunctionComposite<T>(groupName, actionExpression, childFunctions);
            component.IsShowOnNav = hasMenu;
            return component;
        }

        public static void RegisterFunctions(Functions functions)
        {
            functions.IgnoreFunction<AccountController>(t => t.Index())
                .IgnoreFunction<AccountController>(t => t.ChangePassword())
                .IgnoreFunction<LogoutController>(t => t.Index())
                .IgnoreFunction<ErrorController>(t=>t.Forbidden());

            functions.RegisterGroup<HomeController>(
                "首页", t => t.Index(), "icon-home"
                );

            functions.RegisterGroup<UserController>(
                "系统管理", null, "icon-wrench",
                fc<UserController>("用户", t => t.Index(null),
                                   fc<UserController>("新增用户", t => t.Add()),
                                   fc<UserController>("编辑资料", t => t.Edit(null)),
                                   fc<UserController>("修改密码", t => t.ChangePassword(0, null)),
                                   fc<UserController>("删除用户",t=>t.Delete(0))
                    ),
                fc<GroupController>("权限组", t => t.Index(),
                                    fc<GroupController>("编辑分组", t => t.Edit(string.Empty)),
                                    fc<GroupController>("删除分组", t => t.Delete(0))
                    )
                );
        }
    }
}