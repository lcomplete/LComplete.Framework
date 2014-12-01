using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LComplete.Framework.Data
{
    public partial interface IRepository<T> : IQueryable<T>
    {
        /// <summary>
        /// Specifies the related objects to include in the query results.
        /// </summary>
        /// <param name="subSelector">Identifies the field or property to be retrieved. 
        /// If the expression does not identify a field or property that represents a one-to-one or one-to-many relationship, an exception is thrown.</param>
        /// <returns>A new System.Linq.IQueryable&lt;T&gt; with the defined query path.</returns>
        /// <remarks>You cannot specify the loading of two levels of relationships (for example, Orders.OrderDetails).</remarks>
        IQueryable<T> Include(Expression<Func<T, object>> subSelector);

        void Add(T item);

        void Remove(T item);

        /// <summary>
        /// 更新非持久化对象
        /// </summary>
        /// <param name="item">An instance containing updated state</param>
        void Update(T item);

        /// <summary>
        /// 保存持久化对象中的更改
        /// </summary>
        void SaveChanges();
    }
}
