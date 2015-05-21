using System;

namespace LComplete.Framework.Data
{
    /// <summary>
    /// 可排序的分页查询
    /// </summary>
    public class OrderPagingQuery<TModel> : PagingQuery where TModel : class
    {
        public OrderFieldStore<TModel> OrderFieldStore { get; set; }

        private string _order;

        /// <summary>
        /// 排序标识
        /// </summary>
        public String Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OrderFieldStore.ChangeOrderFlags(_order);
            }
        }

        public OrderPagingQuery()
        {
            OrderFieldStore = new OrderFieldStore<TModel>();
        }
    }

    /// <summary>
    /// 可排序的分页查询
    /// </summary>
    public class OrderPagingQuery : OrderPagingQuery<object>
    {

    }
}
