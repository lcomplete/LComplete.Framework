using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LComplete.Framework.Data;

namespace LComplete.Framework.EF
{
    public partial class BaseRepository<TEntities, T> : IRepository<T> where TEntities:DbContext, new() where T:class 
    {
        public BaseRepository()
        {
            this.ActiveContext = new TEntities();
        }

        public TEntities ActiveContext { get; private set; }

        private DbSet<T> _set;

        public DbSet<T> Set
        {
            get
            {
                if (_set == null)
                {
                    _set = this.ActiveContext.Set<T>();
                }
                return _set;
            }
        }

        public void Add(T item)
        {
            this.Set.Add(item);
            this.ActiveContext.SaveChanges();
        }

        public void Remove(T item)
        {
            this.Set.Remove(item);
            this.ActiveContext.SaveChanges();
        }

        public IQueryable<T> Include(Expression<Func<T, object>> subSelector)
        {
            return this.Set.Include(subSelector);
        }

        public void Update(T item)
        {
            this.Set.Attach(item);
            this.ActiveContext.Entry(item).State = EntityState.Modified;
            this.ActiveContext.SaveChanges();
        }

        public void SaveChanges()
        {
            this.ActiveContext.SaveChanges();
        }

        #region Implementation of IEnumerator

        public IEnumerator<T> GetEnumerator()
        {
            return this.Set.AsQueryable().AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IQueryable

        public Type ElementType
        {
            get
            {
                return (this.Set.AsQueryable() as IQueryable).ElementType;
            }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get
            {
                return (this.Set.AsQueryable() as IQueryable).Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return (this.Set.AsQueryable() as IQueryable).Provider;
            }
        }

        #endregion
    }
}
