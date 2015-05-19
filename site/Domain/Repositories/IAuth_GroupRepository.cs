using Domain.Model;
using LComplete.Framework.Data;

namespace Domain.Repositories
{
    public interface IAuth_GroupRepository:IRepository<Auth_Group>
    {
        Auth_Group GetGroup(int id);
        Auth_Group GetGroupByName(string name);
    }

}