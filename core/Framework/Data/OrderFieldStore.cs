using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LComplete.Framework.Common;

namespace LComplete.Framework.Data
{
    /// <summary>
    /// OrderField 集合的封装
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class OrderFieldStore<TModel> where TModel : class
    {
        public IList<OrderField<TModel>> OrderFields { get; private set; }

        public OrderFieldStore()
        {
            OrderFields = new List<OrderField<TModel>>();
        }

        /// <summary>
        /// 添加一个排序字段
        /// </summary>
        /// <param name="fieldExpression"></param>
        /// <param name="orderType"></param>
        /// <param name="priority"></param>
        public void Add(Expression<Func<TModel, object>> fieldExpression, OrderType orderType = OrderType.None, int priority = 0)
        {
            //未提供优先级时，自动设置优先级
            if (priority == 0)
                priority = OrderFields.Count == 0 ? 1 : OrderFields.Max(t => t.Priority) + 1;

            OrderFields.Add(new OrderField<TModel>(fieldExpression, orderType, priority));
        }

        /// <summary>
        /// 添加一个排序字段
        /// </summary>
        /// <param name="orderKey"></param>
        /// <param name="orderType"></param>
        /// <param name="priority"></param>
        public void Add(string orderKey, OrderType orderType = OrderType.None, int priority = 0)
        {
            priority = priority > 0 ? priority : (OrderFields.Max(t => t.Priority) + 1);
            OrderFields.Add(new OrderField<TModel>(orderKey, orderType, priority));
        }

        /// <summary>
        /// 获取排序字段
        /// </summary>
        /// <param name="filedExpression"></param>
        /// <returns></returns>
        public OrderField<TModel> GetOrderField(Expression<Func<TModel, object>> filedExpression)
        {
            string orderKey = ExpressionUtils.ParseMemberName(filedExpression);
            return GetOrderField(orderKey);
        }

        /// <summary>
        /// 获取排序字段
        /// </summary>
        /// <param name="orderKey"></param>
        /// <returns></returns>
        public OrderField<TModel> GetOrderField(string orderKey)
        {
            OrderField<TModel> orderField = OrderFields.FirstOrDefault(t => t.OrderKey == orderKey);
            return orderField;
        }

        /// <summary>
        /// 获取按优先级排序过的排序字段
        /// </summary>
        /// <returns></returns>
        public IList<OrderField<TModel>> GetOrderFieldsByPriority(bool removeNoneOrder = true)
        {
            if (removeNoneOrder)
                return OrderFields.OrderBy(t => t.Priority).Where(t => t.OrderType != OrderType.None).ToList();

            return OrderFields.OrderBy(t => t.Priority).ToList();
        }

        /// <summary>
        /// 设置排序字段
        /// </summary>
        /// <param name="fieldExpression"></param>
        /// <param name="orderType"></param>
        /// <param name="priority"></param>
        public void SetOrderField(Expression<Func<TModel, object>> fieldExpression, OrderType orderType, int priority = 0)
        {
            OrderField<TModel> orderField = GetOrderField(fieldExpression);
            if (orderField != null)
            {
                orderField.OrderType = orderType;
                if (priority > 0)
                    orderField.Priority = priority;
            }
            else
            {
                Add(fieldExpression, orderType, priority);
            }
        }

        /// <summary>
        /// 设置排序字段
        /// </summary>
        /// <param name="orderKey"></param>
        /// <param name="orderType"></param>
        /// <param name="priority"></param>
        public void SetOrderField(string orderKey, OrderType orderType, int priority = 0)
        {
            OrderField<TModel> orderField = GetOrderField(orderKey);
            if (orderField != null)
            {
                orderField.OrderType = orderType;
                if (priority > 0)
                    orderField.Priority = priority;
            }
            else
            {
                Add(orderKey, orderType, priority);
            }
        }

        /// <summary>
        /// 调整排序优先级和排序类型
        /// </summary>
        /// <param name="o">由.号拼接的数字(数字表示索引:从1开始,-表示倒序)</param>
        public void ChangeOrderFlags(string o)
        {
            IList<int> orderFlags = StringParseUtils.ParseJoinString(o, '.').Distinct().ToList();
            IList<int> changedIndexs = new List<int>();

            //设置优先级和排序标识
            int incrementPriority = 1;
            for (int i = 0; i < orderFlags.Count; i++)
            {
                int orderFlag = orderFlags[i];
                int fieldIndex = Math.Abs(orderFlag) - 1;//字符串中的索引标识从1开始
                if (fieldIndex < OrderFields.Count)
                {
                    OrderFields[fieldIndex].Priority = incrementPriority++;
                    OrderFields[fieldIndex].OrderType = orderFlag > 0 ? OrderType.Ascending : OrderType.Descending;
                    changedIndexs.Add(fieldIndex);
                }
            }

            //若调整过排序 则将未调整过的记录中的默认排序去除
            if (changedIndexs.Count > 0)
            {
                for (int i = 0; i < OrderFields.Count; i++)
                {
                    if (!changedIndexs.Contains(i))
                    {
                        OrderFields[i].OrderType = OrderType.None;
                    }
                }
            }

            OrderFields = OrderFields.OrderBy(t => t.Priority).ToList();//重新按优先级排序
        }

        /// <summary>
        /// 获取字符串形式的默认排序标识
        /// </summary>
        /// <returns></returns>
        public string MakeOrderFlags()
        {
            return MakeOrderFlags(string.Empty, OrderType.None);
        }

        /// <summary>
        /// 获取字符串形式的排序标识
        /// </summary>
        /// <param name="fieldExpression">要更改的排序字段的表达式</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="isKeepOtherOrder">其他字段是否保持原始的排序类型</param>
        /// <returns>字符串形式的排序优先级和标识，需要更改的排序对象的优先级最高</returns>
        public string MakeOrderFlags(Expression<Func<TModel, object>> fieldExpression, OrderType orderType, bool isKeepOtherOrder = false)
        {
            string orderKey = ExpressionUtils.ParseMemberName(fieldExpression);
            return MakeOrderFlags(orderKey, orderType, isKeepOtherOrder);
        }

        /// <summary>
        /// 获取字符串形式的排序标识
        /// </summary>
        /// <param name="orderKey">要更改的排序对象的key</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="isKeepOtherOrder">其他字段是否保持原始的排序类型</param>
        /// <returns>字符串形式的排序优先级和标识，需要更改的排序对象的优先级最高</returns>
        public string MakeOrderFlags(string orderKey, OrderType orderType, bool isKeepOtherOrder = false)
        {
            IList<int> orderFlags = new List<int>();
            int makeOrderFlag = 0;

            for (int i = 0; i < OrderFields.Count; i++)
            {
                OrderField<TModel> orderField = OrderFields[i];
                string currentOrderKey = orderField.OrderKey;

                if (currentOrderKey == orderKey)
                {
                    if (orderType != OrderType.None)
                        makeOrderFlag = orderField.RawPriorityIndex*(orderType == OrderType.Descending ? -1 : 1);
                }
                else
                {
                    OrderType otherOrderType = orderField.OrderType;
                    if (isKeepOtherOrder)
                        otherOrderType = orderField.RawOrderType;
                    if (otherOrderType != OrderType.None)
                        orderFlags.Add(orderField.RawPriorityIndex*(otherOrderType == OrderType.Descending ? -1 : 1));
                }
            }

            if (makeOrderFlag != 0)
                orderFlags.Insert(0, makeOrderFlag);

            return string.Join(".", orderFlags.Select(t => t.ToString()));
        }

    }
}