using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LComplete.Framework.Data
{
    public static class PagingDataSourceExtension
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryableSource"></param>
        /// <param name="pagingQuery"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static PagingDataSource<T> QueryPagingDataSource<T>(this IQueryable<T> queryableSource, PagingQuery pagingQuery, int recordCount = 0) where T : class
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
        public static PagingDataSource<T> QueryPagingDataSource<T>(this IQueryable<T> queryableSource, OrderPagingQuery<T> pagingQuery, int recordCount = 0) where T : class
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
                    Expression sortExpression = Unbox(orderField.MemberExpression.Body);
                    Type sortType = sortExpression.Type;//排序字段的实际类型
                    Type funType = typeof(Func<,>);
                    funType = funType.MakeGenericType(typeof(T), sortType);

                    LambdaExpression lambda = Expression.Lambda(funType, sortExpression,
                        orderField.MemberExpression.Parameters);
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

        /// <summary>
        /// list 转换为 PagingDataSource
        /// </summary>
        /// <returns></returns>
        public static PagingDataSource<T> ToPagingDataSource<T>(this IList<T> dataSource, int pageIndex, int pageSize, int recordCount = 0) where T : class
        {
            return new PagingDataSource<T>(dataSource, pageIndex, pageSize, recordCount);
        }

        /// <summary>
        /// list 转换为 PagingDataSource
        /// </summary>
        /// <returns></returns>
        public static PagingDataSource<T> ToPagingDataSource<T>(this IList<T> dataSource, PagingQuery pagingQuery, int recordCount = 0) where T : class
        {
            return new PagingDataSource<T>(dataSource, pagingQuery.Page, pagingQuery.PageSize, recordCount);
        }

        /// <summary>
        /// list 转换为 PagingDataSource
        /// </summary>
        /// <returns></returns>
        private static PagingDataSource<T> ToPagingDataSource<T>(this IList<T> dataSource,
            OrderPagingQuery<T> orderPagingQuery, int recordCount = 0) where T : class
        {
            var pagingData = new PagingDataSource<T>(dataSource, orderPagingQuery.Page, orderPagingQuery.PageSize,
                recordCount);
            pagingData.OrderFieldStore = orderPagingQuery.OrderFieldStore;
            return pagingData;
        }

        /// <summary>
        /// 对list进行排序和分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listSource"></param>
        /// <param name="pagingQuery"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static PagingDataSource<T> ListPagingDataSource<T>(this IList<T> listSource, OrderPagingQuery<T> pagingQuery, int recordCount = 0) where T : class
        {
            if (recordCount == 0 && pagingQuery.IsGetRecordCount)
                recordCount = listSource.Count();

            IOrderedEnumerable<T> query = null;
            foreach (OrderField<T> orderField in pagingQuery.OrderFieldStore.OrderFields)
            {
                if (orderField.OrderType != OrderType.None)
                {
                    if (query != null)
                    {
                        if (orderField.OrderType == OrderType.Ascending)
                        {
                            query = query.ThenBy(orderField.MemberExpression.Compile());
                        }
                        if (orderField.OrderType == OrderType.Descending)
                        {
                            query = query.ThenByDescending(orderField.MemberExpression.Compile());
                        }
                    }
                    else
                    {
                        if (orderField.OrderType == OrderType.Ascending)
                        {
                            query = listSource.OrderBy(orderField.MemberExpression.Compile());
                        }
                        if (orderField.OrderType == OrderType.Descending)
                        {
                            query = listSource.OrderByDescending(orderField.MemberExpression.Compile());
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