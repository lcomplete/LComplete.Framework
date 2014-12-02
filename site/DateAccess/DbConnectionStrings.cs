using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LComplete.Framework.Site.DateAccess
{
    class DbConnectionStrings
    {
        public static string TestDb
        {
            get { return ConfigurationManager.ConnectionStrings["TestDb"].ConnectionString; }
        }
    }
}
