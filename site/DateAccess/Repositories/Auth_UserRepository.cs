using System.Linq;
using Domain.Model;
using Domain.Repositories;

namespace DataAccess.Repositories
{
    public class Auth_UserRepository: Repository<Auth_User>,IAuth_UserRepository
    {
        public Auth_User GetUser(int id)
        {
            return this.FirstOrDefault(t => t.Id == id);
        }

        public Auth_User GetUserByName(string username)
        {
            return this.FirstOrDefault(t => t.Username == username);
        }

        public int GetSuperUserCount()
        {
            return this.Count(t => t.IsSuperUser);
        }
    }
}