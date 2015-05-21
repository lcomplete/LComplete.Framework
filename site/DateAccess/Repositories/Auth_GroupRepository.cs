using System.Linq;
using Domain.Model;
using Domain.Repositories;

namespace DataAccess.Repositories
{
    public class Auth_GroupRepository:Repository<Auth_Group>,IAuth_GroupRepository
    {
        public Auth_Group GetGroup(int id)
        {
            return this.FirstOrDefault(t => t.Id == id);
        }

        public Auth_Group GetGroupByName(string name)
        {
            return this.FirstOrDefault(t => t.Name == name);
        }
    }
}