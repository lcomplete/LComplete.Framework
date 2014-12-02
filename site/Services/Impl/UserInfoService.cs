using System;
using System.Linq;
using LComplete.Framework.CacheHandler;
using LComplete.Framework.Site.Domain.Model;
using LComplete.Framework.Site.Domain.Repositories;

namespace LComplete.Framework.Site.Services.Impl
{
    public class UserInfoService : IUserInfoService
    {
        private IUserInfoRepository _userInfoRepository;

        public UserInfoService(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public UserInfo GetUserInfo(int userId)
        {
            return _userInfoRepository.FirstOrDefault(t => t.Id == userId);
        }

        [Caching]
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}