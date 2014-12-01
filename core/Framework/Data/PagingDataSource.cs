using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LComplete.Framework.Data
{
    /// <summary>
    /// 分页数据源
    /// </summary>
    public class PagingDataSource<T> : PagingModel
    {

        public IList<T> DataSource { get; set; }

        public PagingDataSource(IList<T> dataSource, int pageIndex, int pageSize, int recordCount = 0)
            : base(pageIndex, pageSize, recordCount)
        {
            DataSource = dataSource;

        }

        public PagingDataSource(IList<T> dataSource, PagingQuery pagingQuery, int recordCount = 0) :
            this(dataSource, pagingQuery.Page, pagingQuery.PageSize, recordCount)
        {

        }

    }

    public static class PagingDataSourceExtension
    {
        public static PagingDataSource<T> ToPagingDataSource<T>(this IQueryable<T> queryableSource, PagingQuery pagingQuery, int recordCount = 0)
        {
            if (recordCount == 0 && pagingQuery.IsGetRecordCount)
                recordCount = queryableSource.Count();

            return
                ToPagingDataSource(queryableSource.Skip(pagingQuery.GetSkipCount()).Take(pagingQuery.PageSize).ToList(),
                                   pagingQuery, recordCount);
        }

        /// <summary>
        /// 获取排序并分页的数据 如果有排序条件 会覆盖已有的排序条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryableSource"></param>
        /// <param name="pagingQuery"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static PagingDataSource<T> ToPagingDataSource<T>(this IQueryable<T> queryableSource, OrderPagingQuery<T> pagingQuery, int recordCount = 0) where T : class
        {
            if (recordCount == 0 && pagingQuery.IsGetRecordCount)
                recordCount = queryableSource.Count();

            bool ordered = false;
            foreach (OrderField<T> orderField in pagingQuery.OrderFieldStore.OrderFields)
            {
                string orderType = string.Empty;
                if (orderField.OrderType == OrderType.Descending)
                {
                    orderType = ordered ? "ThenByDescending" : "OrderByDescending";
                }
                else if (orderField.OrderType == OrderType.Ascending)
                {
                    orderType = ordered ? "ThenBy" : "OrderBy";
                }

                if (!string.IsNullOrEmpty(orderType))
                {
                    Expression sortExpression = Unbox(orderField.FieldExpression.Body);
                    Type sortType = sortExpression.Type;//排序字段的实际类型
                    Type funType = typeof(Func<,>);
                    funType = funType.MakeGenericType(typeof(T), sortType);

                    LambdaExpression lambda = Expression.Lambda(funType, sortExpression,
                                                                orderField.FieldExpression.Parameters);
                    MethodCallExpression method = Expression.Call(typeof(Queryable), orderType,
                                                                  new Type[] { typeof(T), sortExpression.Type },
                                                                  queryableSource.Expression, lambda);

                    queryableSource = queryableSource.Provider.CreateQuery<T>(method);
                    ordered = true;
                }
            }

            return
                ToPagingDataSource(queryableSource.Skip(pagingQuery.GetSkipCount()).Take(pagingQuery.PageSize).ToList(),
                                   pagingQuery, recordCount);
        }

        /// <summary>
        /// 为参数拆箱。
        /// </summary>
        private static Expression Unbox(Expression property)
        {
            Expression result;

            UnaryExpression convert = property as UnaryExpression;
            if (convert == null)
            {
                result = property;
            }
            else
            {
                result = Unbox(convert.Operand);
            }

            return result;
        }

        public static PagingDataSource<T> ToPagingDataSource<T>(this IList<T> dataSource, int pageIndex, int pageSize, int recordCount = 0)
        {
            return new PagingDataSource<T>(dataSource, pageIndex, pageSize, recordCount);
        }

        public static PagingDataSource<T> ToPagingDataSource<T>(this IList<T> dataSource, PagingQuery pagingQuery, int recordCount = 0)
        {
            return new PagingDataSource<T>(dataSource, pagingQuery.Page, pagingQuery.PageSize, recordCount);
        }

        /// <summary>
        /// 对list进行排序和分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listSource"></param>
        /// <param name="pagingQuery"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static PagingDataSource<T> ListToPagingDataSource<T>(this IList<T> listSource, OrderPagingQuery<T> pagingQuery, int recordCount = 0) where T : class
        {
            if (recordCount == 0 && pagingQuery.IsGetRecordCount)
                recordCount = listSource.Count();

            IOrderedEnumerable<T> query=null;
            foreach (OrderField<T> orderField in pagingQuery.OrderFieldStore.OrderFields)
            {
                if (orderField.OrderType!=OrderType.None)
                {
                    if (query!=null)
                    {
                        if (orderField.OrderType == OrderType.Ascending)
                        {
                            query = query.ThenBy(orderField.FieldExpression.Compile());
                        }
                        if (orderField.OrderType == OrderType.Descending)
                        {
                            query = query.ThenByDescending(orderField.FieldExpression.Compile());
                        }
                    }
                    else
                    {
                        if (orderField.OrderType == OrderType.Ascending)
                        {
                            query = listSource.OrderBy(orderField.FieldExpression.Compile());
                        }
                        if (orderField.OrderType == OrderType.Descending)
                        {
                            query = listSource.OrderByDescending(orderField.FieldExpression.Compile());
                        }
                    }
                }
            }

            return
                ToPagingDataSource(query.Skip(pagingQuery.GetSkipCount()).Take(pagingQuery.PageSize).ToList(),
                                   pagingQuery, recordCount);
        }
    }
}
