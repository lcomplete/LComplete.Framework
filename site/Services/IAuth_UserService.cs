using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Exception;
using Domain.InfoModel;
using Domain.Model;
using LComplete.Framework.Exceptions;

namespace Services
{
    public interface IAuth_UserService
    {
        Auth_User GetUser(int id, Expression<Func<Auth_User,object>> subSelector=null);

        Auth_User AddNewUser(string username,string password);

        IList<Auth_User> GetUsers(string username);

        void ChangePassword(int id, string password);

        /// <summary>
        /// 更新用户资料
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="DataAlreadyExistsException"></exception>
        /// <exception cref="UnAcceptOperationException"></exception>
        Auth_User UpdateProfile(Auth_UserProfile user);

        Auth_User ValidateUser(string username, string password);

        HashSet<string> GetUserPermissionKeys(int userId);

        void UpdateLoginDate(int id, DateTime loginDate);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="UnAcceptOperationException"></exception>
        void DeleteUser(int id);
    }
}
