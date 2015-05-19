using Domain.Repositories;

namespace Services.Impl
{
    public class Auth_User_GroupsService : IAuth_User_GroupsService
    {
        private IAuth_User_GroupsRepository _userGroupsRepository;

        public Auth_User_GroupsService(IAuth_User_GroupsRepository userGroupsRepository)
        {
            _userGroupsRepository = userGroupsRepository;
        }
    }
}