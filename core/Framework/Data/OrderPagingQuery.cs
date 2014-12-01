using System;

namespace LComplete.Framework.Data
{
    /// <summary>
    /// 可排序的分页查询
    /// </summary>
    public class OrderPagingQuery<TModel>:PagingQuery where TModel:class 
    {
        public OrderFieldStore<TModel> OrderFieldStore { get; private set; }

        private string _o;

        /// <summary>
        /// 排序标识
        /// </summary>
        public String O
        {
            get { return _o; }
            set
            {
                _o = value;
                OrderFieldStore.ChangeOrderFlags(_o);
            }
        }

        public OrderPagingQuery()
        {
            OrderFieldStore=new OrderFieldStore<TModel>();
        }
    }
}
