using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LComplete.Framework.Common;

namespace LComplete.Framework.Data
{
    public static class OrderKeyParser
    {
        public static string ParseOrderKey<T>(Expression<Func<T, object>> fieldExpression) where T : class
        {
            MemberExpression memberExpression; 
            var operation = fieldExpression.Body as UnaryExpression;
            if(operation!=null)
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
            return memberInfo.Name;
        }
    }

    public class OrderField<TModel> where TModel : class
    {
        public OrderField(Expression<Func<TModel, object>> fieldExpression, OrderType orderType = OrderType.None, int priority = 0,string fieldText="")
        {
            FieldExpression = fieldExpression;
            Priority = priority;
            RawPriority = priority;
            OrderType = orderType;
            FieldText = fieldText;

            _orderKey = OrderKeyParser.ParseOrderKey(fieldExpression);
        }

        public Expression<Func<TModel, Object>> FieldExpression { get; private set; }

        private readonly string _orderKey;
        public string OrderKey { get { return _orderKey; } }

        public OrderType OrderType { get; set; }

        /// <summary>
        /// 优先级 数字越小优先级越高
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 原始优先级
        /// </summary>
        public int RawPriority { get; private set; }

        public string FieldText { get; set; }
    }

    public class OrderFieldStore<TModel> where TModel : class
    {
        private IList<OrderField<TModel>> _orderFields;

        public IList<OrderField<TModel>> OrderFields
        {
            get { return _orderFields; }
        }

        public OrderFieldStore()
        {
            _orderFields = new List<OrderField<TModel>>();
        }

        public void Add(Expression<Func<TModel, object>> fieldExpression, OrderType orderType = OrderType.None)
        {
            int priority = _orderFields.Count+1;
            _orderFields.Add(new OrderField<TModel>(fieldExpression, orderType, priority));
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
                int fieldIndex = Math.Abs(orderFlag)-1;//字符串中的索引标识从1开始
                if (fieldIndex <= _orderFields.Count)
                {
                    _orderFields[fieldIndex].Priority = incrementPriority++;
                    _orderFields[fieldIndex].OrderType = orderFlag > 0 ? OrderType.Ascending : OrderType.Descending;
                    changedIndexs.Add(fieldIndex);
                }
            }

            //若调整过排序 则将未调整过的记录中的默认排序去除
            if (changedIndexs.Count > 0)
            {
                for (int i = 0; i < _orderFields.Count; i++)
                {
                    if (!changedIndexs.Contains(i))
                    {
                        _orderFields[i].OrderType = OrderType.None;
                    }
                }
            }

            _orderFields = _orderFields.OrderBy(t=>t.Priority).ToList();//重新按优先级排序
        }

        /// <summary>
        /// 获取字符串形式的排序标识
        /// </summary>
        /// <returns></returns>
        public string GetOrderFlags()
        {
            return MakeOrderFlags(string.Empty, OrderType.None);
        }

        public OrderField<TModel> GetOrderField(Expression<Func<TModel,object>> filedExpression)
        {
            string orderKey = OrderKeyParser.ParseOrderKey(filedExpression);
            OrderField<TModel> orderField = _orderFields.FirstOrDefault(t => t.OrderKey == orderKey);
            return orderField;
        }

        /// <summary>
        /// 获取字符串形式的排序标识
        /// </summary>
        /// <param name="orderKey">要更改的排序对象的key</param>
        /// <param name="orderType">排序类型</param>
        /// <returns>字符串形式的排序优先级和标识，需要更改的排序对象的优先级最高</returns>
        public string MakeOrderFlags(string orderKey, OrderType orderType)
        {
            IList<int> orderFlags = new List<int>();
            int makeOrderFlag = 0;

            for (int i = 0; i < _orderFields.Count; i++)
            {
                OrderField<TModel> orderField = _orderFields[i];
                string currentOrderKey = orderField.OrderKey;

                if (currentOrderKey == orderKey)
                {
                    if(orderType != OrderType.None)
                        makeOrderFlag = orderField.RawPriority * (orderType == OrderType.Descending ? -1 : 1);
                }
                else if (orderField.OrderType != OrderType.None)
                {
                    orderFlags.Add(orderField.RawPriority*(orderField.OrderType == OrderType.Descending ? -1 : 1));
                }
            }

            if (makeOrderFlag != 0)
                orderFlags.Insert(0, makeOrderFlag);

            return string.Join(".", orderFlags.Select(t => t.ToString()));
        }

    }

    public enum OrderType
    {
        None,
        Ascending,
        Descending
    }
}
