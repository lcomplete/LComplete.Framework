using System;
using System.Linq.Expressions;
using LComplete.Framework.Common;

namespace LComplete.Framework.Data
{
    /// <summary>
    /// 排序字段
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class OrderField<TModel> where TModel : class
    {
        public OrderField(Expression<Func<TModel, object>> memberExpression, OrderType orderType = OrderType.None, int priorityIndex = 0)
        {
            MemberExpression = memberExpression;
            OrderKey = ExpressionUtils.ParseMemberName(memberExpression);

            Priority = priorityIndex;
            RawPriorityIndex = priorityIndex;
            OrderType = orderType;
            RawOrderType = orderType;
        }

        public OrderField(string orderKey, OrderType orderType = OrderType.None, int priorityIndex = 0)
        {
            OrderKey = orderKey;

            Priority = priorityIndex;
            RawPriorityIndex = priorityIndex;
            OrderType = orderType;
            RawOrderType = orderType;
        }
        
        /// <summary>
        /// 成员表达式
        /// </summary>
        public Expression<Func<TModel, Object>> MemberExpression { get; private set; }

        public string OrderKey { get; private set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 默认（原始）排序类型
        /// </summary>
        public OrderType RawOrderType { get;private set; }

        /// <summary>
        /// 优先级 数字越小优先级越高
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 默认（原始）优先级索引
        /// </summary>
        public int RawPriorityIndex { get; private set; }

    }

    public enum OrderType
    {
        None,
        Ascending,
        Descending
    }
}
