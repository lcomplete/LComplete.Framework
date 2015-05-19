using System.Collections.Generic;
using Domain.Model;
using LComplete.Framework.Data;

namespace Domain.Repositories
{
    public interface IAuth_User_GroupsRepository:IRepository<Auth_User_Groups>
    {
        IList<Auth_User_Groups> GetUserGroups(int userId);
    }
}