using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Domain.Model;
using LComplete.Framework.IoC;
using LComplete.Framework.Web.Auth;
using LComplete.Framework.Web.Common;
using Services;

namespace Admin.Extensions
{
    public static class PermissionExtensions
    {
        private const string PERMISSION_ITEM_KEY = "Auth_Permissions";
        private const string SUPER_USER_KEY = "SUPER_USER";
        private const string CURRENT_FUNCTION = "Current_ACTION_FUNCTION";
        private const string FUNCTION_KEY = "Current_Function_Key";
        private const string USER_KEY = "USER_DATA_KEY";

        /// <summary>
        /// 设置权限相关数据
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="filterContext"> </param>
        public static void SetPermissionData(this ControllerBase controller,AuthorizationContext filterContext)
        {
            int userId = AuthenticationUtils.GetLoginUserId();
            var userService = ContainerManager.Resolve<IAuth_UserService>();
            Auth_User user = userService.GetUser(userId);
            bool isSuperUser = false;
            if (user != null)
            {
                isSuperUser = user.IsSuperUser;
                controller.ViewData[USER_KEY] = user;
            }

            //权限集合
            Dictionary<string, FunctionComponent> functions;
            if(isSuperUser)
            {
                functions = FunctionTable.Functions.GetAllFunction();
            }
            else
            {
                HashSet<string> permissionKeys = userService.GetUserPermissionKeys(userId);
                functions = FunctionTable.Functions.MapFunction(permissionKeys);
            }

            //当前Action对应的功能
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string functionKey = controllerName + "." + actionName;
            FunctionComponent currentFunction;
            if(functions.TryGetValue(functionKey, out currentFunction))
            {
                controller.ViewData[CURRENT_FUNCTION] = currentFunction;
                controller.ViewBag.Title = currentFunction.Name;
            }

            controller.ViewData[FUNCTION_KEY] = functionKey;
            controller.ViewData[SUPER_USER_KEY] = isSuperUser;
            controller.ViewData[PERMISSION_ITEM_KEY] = functions;
        }

        public static Auth_User GetCurrentUser(this ControllerBase controller)
        {
            object objUser;
            controller.ViewData.TryGetValue(USER_KEY, out objUser);
            return objUser as Auth_User;
        }

        public static FunctionComponent GetCurrentFunction(this ControllerBase controller)
        {
            if(controller.ViewData.ContainsKey(CURRENT_FUNCTION))
                return controller.ViewData[CURRENT_FUNCTION] as FunctionComponent;
            return null;
        }

        public static Dictionary<string, FunctionComponent> GetCurrentPermissions(this ControllerBase controller)
        {
            return controller.ViewData[PERMISSION_ITEM_KEY] as Dictionary<string, FunctionComponent>;
        } 

        public static bool HasAnyPermission(this ControllerBase controller,IList<FunctionComponent> functions)
        {
            if (controller.HasSuperPermission())
                return true;

            Dictionary<string, FunctionComponent> permissions = controller.GetCurrentPermissions();
            bool hasPermission = false;
            if (functions != null)
            {
                foreach (FunctionComponent component in functions)
                {
                    if (permissions.ContainsKey(component.FunctionKey))
                    {
                        hasPermission = true;
                        break;
                    }
                }
            }
            return hasPermission;
        }

        public static bool HasSuperPermission(this ControllerBase controller)
        {
            if(controller.ViewData.ContainsKey(SUPER_USER_KEY))
                return (bool)controller.ViewData[SUPER_USER_KEY];

            return false;
        }

        public static bool HasPermission(this ControllerBase controller)
        {
            return controller.HasPermission(controller.ViewData[FUNCTION_KEY].ToString());
        }

        public static bool HasPermission(this ControllerBase controller, string functionKey)
        {
            if (controller.HasSuperPermission())
                return true;
            if (FunctionTable.Functions.IsIgnoreFunction(functionKey))
                return true;

            Dictionary<string, FunctionComponent> permissions = controller.GetCurrentPermissions();
            return permissions.ContainsKey(functionKey);
        }

        public static bool HasPermission<T>(this ControllerBase controller,Expression<Func<T, object>> actionExpression) where T:Controller
        {
            ActionIdentity actionIdentity = ActionExpressionParser.AnalysisExpression(actionExpression);
            return HasPermission(controller, actionIdentity.FunctionKey);
        }
    }
}