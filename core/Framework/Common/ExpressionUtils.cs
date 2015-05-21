using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LComplete.Framework.Common
{
    /// <summary>
    /// 表达式树工具类
    /// </summary>
    public static class ExpressionUtils
    {
        /// <summary>
        /// 解析成员名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public static string ParseMemberName<T>(Expression<Func<T, object>> memberExpression) where T : class
        {
            MemberInfo memberInfo = ParseMemberInfo(memberExpression);
            return memberInfo.Name;
        }

        /// <summary>
        /// 解析成员信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldExpression"></param>
        /// <returns></returns>
        public static MemberInfo ParseMemberInfo<T>(Expression<Func<T, object>> fieldExpression) where T : class
        {
            MemberExpression memberExpression;
            var operation = fieldExpression.Body as UnaryExpression;
            if (operation != null)
            {
                memberExpression = operation.Operand as MemberExpression;
            }
            else
            {
                memberExpression = fieldExpression.Body as MemberExpression;
            }

            if (memberExpression == null)
                throw new ArgumentException("请使用属性表达式", "fieldExpression");

            MemberInfo memberInfo = memberExpression.Member;
            return memberInfo;
        }
    }
}