using System;
using System.Collections;
using System.Collections.Generic;
using LComplete.Framework.Data;
using LComplete.Framework.Site.Domain.QueryCondition;
using LComplete.Framework.Site.Domain.ViewModel;

namespace LComplete.Framework.Site.Services.Impl
{
    public class CustomService : ICustomService
    {
        public PagingDataSource<CustomModel> GetCustomList(CustomOrderQuery customOrderQuery)
        {
            return GetTestList().ListPagingDataSource(customOrderQuery);
        }

        private IList<CustomModel> GetTestList()
        {
            int size = 1000;
            var list = new List<CustomModel>(size);
            for (int i = 0; i < 1000; i++)
            {
                int id = i%100;
                string name = ((char) i%127).ToString() + i + "name";
                string title = "title" + i;
                DateTime createDate = DateTime.Now.Date.AddDays(-i%7);
                var model = new CustomModel()
                {
                    Id = id,
                    Name = name,
                    Title = title,
                    CreateDate = createDate
                };
                list.Add(model);
            }
            return list;
        }
    }
}