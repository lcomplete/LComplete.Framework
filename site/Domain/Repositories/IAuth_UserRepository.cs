using Domain.Model;
using LComplete.Framework.Data;

namespace Domain.Repositories
{
    public interface IAuth_UserRepository:IRepository<Auth_User>
    {
        Auth_User GetUser(int id);

        Auth_User GetUserByName(string username);

        int GetSuperUserCount();
    }
}