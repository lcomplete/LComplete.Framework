using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.EF;

namespace LComplete.Framework.Site.DateAccess
{
    public class Repository<T>:BaseRepository<TestDbContext,T> where T : class, new()
    {
    }
}
