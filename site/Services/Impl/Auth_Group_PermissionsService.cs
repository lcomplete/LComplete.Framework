using Domain.Repositories;

namespace Services.Impl
{
    public class Auth_Group_PermissionsService:IAuth_Group_PermissionsService
    {
        private IAuth_Group_PermissionsRepository _groupPermissionsRepository;

        public Auth_Group_PermissionsService(IAuth_Group_PermissionsRepository groupPermissionsRepository)
        {
            _groupPermissionsRepository = groupPermissionsRepository;
        }
    }
}