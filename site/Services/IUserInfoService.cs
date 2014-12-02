using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.Site.Domain.Model;

namespace LComplete.Framework.Site.Services
{
    public interface IUserInfoService
    {
        UserInfo GetUserInfo(int userId);

        DateTime GetCurrentTime();
    }
}
