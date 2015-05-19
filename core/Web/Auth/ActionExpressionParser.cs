using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LComplete.Framework.Web.Auth
{
    public static class ActionExpressionParser
    {
        /// <summary>
        /// 解析表达式树
        /// </summary>
        public static ActionIdentity AnalysisExpression<T>(Expression<Func<T, object>> actionExpression)
        {
            if (actionExpression == null)
                return null;

            //使用一元表达式的解析方法
            //UnaryExpression operation = (UnaryExpression)actionExpression.Body;
            //MethodCallExpression methodCall = (MethodCallExpression)operation.Operand;
            //ConstantExpression methodConstant = (ConstantExpression)methodCall.Arguments[2];

            MethodCallExpression methodCall = (MethodCallExpression)actionExpression.Body;
            MethodInfo methodInfo = methodCall.Method;

            string _controllerName = methodInfo.DeclaringType.Name;
            _controllerName = _controllerName.Substring(0, _controllerName.Length - "controller".Length);
            string _actionName = methodInfo.Name;
            string _controllerTypeName = methodInfo.DeclaringType.FullName;
            string _functionKey = _controllerTypeName + "." + _actionName;

            ActionIdentity identity=new ActionIdentity()
                                        {
                                            ControllerName = _controllerName,
                                            ActionName = _actionName,
                                            FunctionKey = _functionKey,
                                            ControllerTypeName = _controllerTypeName,
                                            AreaName = string.Empty
                                        };
            const string areasNamespace = ".Areas.";
            const string controllerNamespace = ".Controllers.";
            if (_controllerTypeName.Contains(areasNamespace))
            {
                int areaPosition = _controllerTypeName.IndexOf(areasNamespace) + areasNamespace.Length;
                identity.AreaName =
                    _controllerTypeName.Substring(areaPosition,
                                                  _controllerTypeName.IndexOf(controllerNamespace) - areaPosition);
            }
            return identity;
        }
    }
}
