using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.Data;
using LComplete.Framework.Site.Domain.ViewModel;

namespace LComplete.Framework.Site.Domain.QueryCondition
{
    public class CustomOrderQuery : OrderPagingQuery<CustomModel>
    {
        public CustomOrderQuery()
        {
            OrderFieldStore.Add(t => t.Id, OrderType.Ascending);
            OrderFieldStore.Add(t => t.Name);
            OrderFieldStore.Add(t => t.CreateDate);
            OrderFieldStore.Add(t => t.Title);
        }
    }
}
