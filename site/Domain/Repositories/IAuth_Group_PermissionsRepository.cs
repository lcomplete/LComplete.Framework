using System.Collections.Generic;
using Domain.Model;
using LComplete.Framework.Data;

namespace Domain.Repositories
{
    public interface IAuth_Group_PermissionsRepository:IRepository<Auth_Group_Permissions>
    {
        IList<Auth_Group_Permissions> GetPermissionsByGroupId(int id);

        void DeleteByGroupId(int id);
    }
}