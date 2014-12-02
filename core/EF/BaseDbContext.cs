using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

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
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
