using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.Common;
using LComplete.Framework.Dapper;
using LComplete.Framework.Data;
using LComplete.Framework.Site.Domain.Model;
using LComplete.Framework.Site.Domain.Repositories;

namespace LComplete.Framework.Site.DateAccess
{
    public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
    {
        public UserInfo GetUserInfoBySqlHelper()
        {
            using (
                IDataReader reader = SqlHelper.ExecuteReader(DbConnectionStrings.TestDb, CommandType.Text,
                    "SELECT TOP 1 * FROM UserInfo"))
            {
                return OrmUtils.ReadToEntity<UserInfo>(reader);
            }
        }

        public UserInfo GetUserInfoByEFSql()
        {
            return ActiveContext.Database.SqlQuery<UserInfo>("SELECT TOP 1 * FROM UserInfo").FirstOrDefault();
        }

        public UserInfo GetUserInfoByDapper()
        {
            return
                DataUtils.Query<UserInfo>(DbConnectionStrings.TestDb, "SELECT TOP 1 * FROM UserInfo").FirstOrDefault();
        }

        public UserInfo GetUserInfo()
        {
            return this.FirstOrDefault();
        }
    }
}
