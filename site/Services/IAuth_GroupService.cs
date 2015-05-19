using System.Collections.Generic;
using Domain.Model;

namespace Services
{
    public interface IAuth_GroupService
    {
        IList<Auth_Group> GetGroups();

        Auth_Group GetGroup(int id);

        void Save(Auth_Group authGroup);

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1 删除成功，-1 分组下有用户无法删除，0 分组不存在</returns>
        int Delete(int id);
    }
}