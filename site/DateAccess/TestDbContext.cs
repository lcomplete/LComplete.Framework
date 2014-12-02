using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.EF;
using LComplete.Framework.Site.Domain.Model;

namespace LComplete.Framework.Site.DateAccess
{
    public class TestDbContext : BaseDbContext
    {
        public TestDbContext() : base("TestDb") { }

        static TestDbContext()
        {
            Database.SetInitializer<TestDbContext>(null);
        }

        public DbSet<UserInfo> UserInfos { get; set; }
    }
}
