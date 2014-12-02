using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.Site.Domain.Model;
using LComplete.Framework.Site.Domain.Repositories;

namespace LComplete.Framework.Site.DateAccess
{
    public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
    {
    }
}
