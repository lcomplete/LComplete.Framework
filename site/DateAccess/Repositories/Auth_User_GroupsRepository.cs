using System.Collections.Generic;
using System.Linq;
using Domain.Model;
using Domain.Repositories;

namespace DataAccess.Repositories
{
    public class Auth_User_GroupsRepository:Repository<Auth_User_Groups>,IAuth_User_GroupsRepository
    {
        public IList<Auth_User_Groups> GetUserGroups(int userId)
        {
            return this.Where(t => t.UserId == userId).ToList();
        }
    }
}