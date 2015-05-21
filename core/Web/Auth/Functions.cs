using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LComplete.Framework.Web.Auth
{
    public class Functions
    {
        public IList<FunctionComponent> FunctionGroup { get; private set; }

        private Dictionary<string, FunctionComponent> FunctionDict { get; set; }

        private Dictionary<string, FunctionComponent> IgnoreDict { get; set; }

        public Functions()
        {
            FunctionGroup = new List<FunctionComponent>();
            FunctionDict = new Dictionary<string, FunctionComponent>();
            IgnoreDict = new Dictionary<string, FunctionComponent>();
        }

        /// <summary>
        /// 注册功能分组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupName"></param>
        /// <param name="actionExpression"></param>
        /// <param name="childFunctions"></param>
        /// <exception cref="ArgumentException">不能注册重复的功能项</exception>
        public void RegisterGroup<T>(string groupName, Expression<Func<T, object>> actionExpression, params FunctionComponent[] childFunctions) where T : Controller
        {
            RegisterGroup(groupName, actionExpression, null, true, childFunctions);
        }

        public void RegisterGroup<T>(string groupName, Expression<Func<T, object>> actionExpression, string iconClass, params FunctionComponent[] childFunctions) where T : Controller
        {
            RegisterGroup(groupName, actionExpression, iconClass, true, childFunctions);
        }

        public void RegisterGroup<T>(string groupName, Expression<Func<T, object>> actionExpression, string iconClass, bool hasMenu, params FunctionComponent[] childFunctions) where T : Controller
        {
            FunctionComponent component = new FunctionComposite<T>(groupName, actionExpression, childFunctions);
            component.IconClass = iconClass;
            component.IsShowOnNav = hasMenu;
            FunctionGroup.Add(component);

            RegisterFunctionDict(component);
        }

        /// <summary>
        /// 注册需要忽略的功能（这些功能任何登陆用户都可使用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionExpression"></param>
        public Functions IgnoreFunction<T>(Expression<Func<T, object>> actionExpression) where T : Controller
        {
            FunctionComponent component = new FunctionComposite<T>(string.Empty, actionExpression);
            AddIgnoreFunction(component);
            return this;
        }

        private void AddIgnoreFunction(FunctionComponent component)
        {
            if (!IgnoreDict.ContainsKey(component.FunctionKey))
            {
                IgnoreDict.Add(component.FunctionKey, component);
            }
        }

        /// <summary>
        /// 注册功能到字典中
        /// </summary>
        /// <param name="component"></param>
        /// <exception cref="ArgumentException">不能注册重复的功能项</exception>
        private void RegisterFunctionDict(FunctionComponent component)
        {
            if (!string.IsNullOrEmpty(component.FunctionKey))
            {
                if (FunctionDict.ContainsKey(component.FunctionKey))
                    throw new ArgumentException("不能注册重复的功能项");

                FunctionDict.Add(component.FunctionKey, component);
            }

            if (component.ChildFunctionItems != null)
            {
                foreach (var childFunction in component.ChildFunctionItems)
                {
                    RegisterFunctionDict(childFunction);
                }
            }
        }

        /// <summary>
        /// 获取功能
        /// </summary>
        /// <param name="functionKey"></param>
        /// <returns></returns>
        public FunctionComponent GetFunction(string functionKey)
        {
            FunctionComponent function;
            if (FunctionDict.TryGetValue(functionKey, out function))
                return function;

            return null;
        }

        /// <summary>
        /// 从权限键中获取对应的功能列表
        /// </summary>
        /// <param name="permissionKeys"></param>
        /// <returns></returns>
        public Dictionary<string, FunctionComponent> MapFunction(HashSet<string> permissionKeys)
        {
            var functions = new Dictionary<string, FunctionComponent>();
            foreach (string permissionKey in permissionKeys)
            {
                FunctionComponent function;
                if (FunctionDict.TryGetValue(permissionKey, out function))
                    functions.Add(permissionKey, function);
            }
            return functions;
        }

        public Dictionary<string, FunctionComponent> GetAllFunction()
        {
            return new Dictionary<string, FunctionComponent>(FunctionDict);
        }

        public bool IsIgnoreFunction(string functionKey)
        {
            return IgnoreDict.ContainsKey(functionKey);
        }
    }
}
