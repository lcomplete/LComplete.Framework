using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.Data;
using LComplete.Framework.Site.Domain.QueryCondition;
using LComplete.Framework.Site.Domain.ViewModel;

namespace LComplete.Framework.Site.Services
{
    public interface ICustomService
    {
        PagingDataSource<CustomModel> GetCustomList(CustomOrderQuery customOrderQuery);
    }
}
