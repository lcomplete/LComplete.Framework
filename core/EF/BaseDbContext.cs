using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using LComplete.Framework.Data;

namespace LComplete.Framework.EF
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext()
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        static BaseDbContext()
        {
            Database.SetInitializer<BaseDbContext>(null);
        }

        public BaseDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public PagingDataSource<T> GetPagingDataSource<T>(string withSql, OrderPagingQuery<T> query,
            string tableName = "TResult") where T : class
        {
            string pageSql = SqlBuildHelper.BuildPageSql(withSql, query, tableName);
            string countSql = SqlBuildHelper.BuildCountSql(withSql, tableName);
            IList<T> pagedList = Database.SqlQuery<T>(pageSql).ToList();
            int count = Database.SqlQuery<int>(countSql).FirstOrDefault();
            var result = new PagingDataSource<T>(pagedList, query, count);
            return result;
        }
    }
}
