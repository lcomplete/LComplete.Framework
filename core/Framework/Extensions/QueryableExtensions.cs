using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LComplete.Framework.Extensions
{
    /// <summary>
    /// 可查询对象 扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 当condition为true时才应用predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySource"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereCondition<T>(this IQueryable<T> querySource,
            Expression<Func<T, bool>> predicate, bool condition)
        {
            if (condition)
                return querySource.Where(predicate);

            return querySource;
        }

        /// <summary>
        /// 当objCompare不为null时才应用predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySource"></param>
        /// <param name="predicate"></param>
        /// <param name="objCompare"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereWhenNotNull<T>(this IQueryable<T> querySource,
            Expression<Func<T, bool>> predicate, object objCompare)
        {
            return WhereCondition(querySource, predicate, objCompare != null);
        }
    }
}
