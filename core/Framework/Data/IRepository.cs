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
        /// 指定关联对象 加载到结果集中
        /// </summary>
        /// <param name="subSelector">关联对象</param>
        /// <returns>System.Linq.IQueryable&lt;T&gt; 包含指定的关联对象.</returns>
        /// <remarks>可以指定对象关系 (for example, Orders.OrderDetails).</remarks>
        IQueryable<T> Include(Expression<Func<T, object>> subSelector);

        /// <summary>
        /// 将对象添加到数据库
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// 将对象从数据库中移除
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);

        /// <summary>
        /// 更新非持久化对象
        /// </summary>
        /// <param name="item">需要更新的对象</param>
        void Update(T item);

        /// <summary>
        /// 保存持久化对象中的更改
        /// </summary>
        void SaveChanges();
    }
}
