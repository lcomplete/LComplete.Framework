using System.Collections.Generic;
using System.Linq;
using Domain.Model;
using Domain.Repositories;

namespace DataAccess.Repositories
{
    public class Auth_Group_PermissionsRepository : Repository<Auth_Group_Permissions>,
                                                    IAuth_Group_PermissionsRepository
    {
        public IList<Auth_Group_Permissions> GetPermissionsByGroupId(int id)
        {
            return this.Where(t => t.GroupId == id).ToList();
        }

        public void DeleteByGroupId(int id)
        {
            IList<Auth_Group_Permissions> permissions = GetPermissionsByGroupId(id);
            foreach (Auth_Group_Permissions permission in permissions)
            {
                this.Set.Remove(permission);
            }
            SaveChanges();
        }
    }

}