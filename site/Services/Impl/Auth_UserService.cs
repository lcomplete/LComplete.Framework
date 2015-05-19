using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Domain.Exception;
using Domain.InfoModel;
using Domain.Model;
using Domain.Repositories;
using LComplete.Framework.Common;
using LComplete.Framework.Exceptions;

namespace Services.Impl
{
    public class Auth_UserService : IAuth_UserService
    {
        private IAuth_UserRepository _userRepository;
        private IAuth_User_GroupsRepository _userGroupsRepository;

        public Auth_UserService(IAuth_UserRepository userRepository,IAuth_User_GroupsRepository userGroupsRepository)
        {
            _userRepository = userRepository;
            _userGroupsRepository = userGroupsRepository;
        }

        public Auth_User GetUser(int id, Expression<Func<Auth_User, object>> subSelector = null)
        {
            if (subSelector!=null)
                _userRepository.Include(subSelector);
            return _userRepository.GetUser(id);
        }

        public Auth_User AddNewUser(string username, string password)
        {
            Auth_User existsUser = _userRepository.GetUserByName(username);
            if(existsUser!=null)
            {
                throw new DataAlreadyExistsException("该用户名已经存在");
            }

            string passwordSalt = EncryptUtils.GenerateSalt();
            password = EncryptUtils.EncodePassword(password, passwordSalt);

            Auth_User user = new Auth_User()
                               {
                                   CreateDate = DateTime.Now,
                                   Email = string.Empty,
                                   IsEnabled = false,
                                   IsSuperUser = false,
                                   LastLoginDate = DateTime.Now,
                                   Password = password,
                                   RealName = string.Empty,
                                   Username = username,
                                   PasswordSalt = passwordSalt
                               };
            _userRepository.Add(user);
            return user;
        }

        public IList<Auth_User> GetUsers(string username)
        {
            if (!string.IsNullOrEmpty(username))
                return _userRepository.Where(t => t.Username.Contains(username)).ToList();
            return _userRepository.ToList();
        }

        public void ChangePassword(int id, string password)
        {
            var user = _userRepository.GetUser(id);
            user.Password = EncryptUtils.EncodePassword(password, user.PasswordSalt);
            _userRepository.SaveChanges();
        }

        public Auth_User UpdateProfile(Auth_UserProfile userProfile)
        {
            var existsUser = _userRepository.GetUserByName(userProfile.Username);
            if(existsUser!=null && existsUser.Id!=userProfile.Id)
            {
                throw new DataAlreadyExistsException("用户名已经存在");
            }

            Auth_User user;
            if (existsUser != null)
                user = existsUser;
            else
                user = _userRepository.GetUser(userProfile.Id);

            if(user.IsSuperUser && !userProfile.IsSuperUser) //取消超级管理员时 检查是否还有超级管理员
            {
                ThrowWhenLessThanTwoSuperUser();
            }

            IList<Auth_User_Groups> userGroupses = _userGroupsRepository.GetUserGroups(user.Id);
            foreach (var userGroup in userGroupses)
            {
                if(userProfile.SelectGroupIds.Contains(userGroup.GroupId))
                {
                    userProfile.SelectGroupIds.Remove(userGroup.GroupId);
                }
                else
                {
                    _userGroupsRepository.Remove(userGroup);
                }
            }
            foreach (var newGroupId in userProfile.SelectGroupIds)
            {
                _userGroupsRepository.Add(new Auth_User_Groups()
                                              {
                                                  UserId = user.Id,
                                                  GroupId = newGroupId
                                              });
            }

            Mapper.Map(userProfile, user);
            _userRepository.SaveChanges();

            return user;
        }

        private void ThrowWhenLessThanTwoSuperUser()
        {
            int superUserCount = _userRepository.GetSuperUserCount();
            if (superUserCount <= 1)
                throw new UnAcceptOperationException((int)ErrorCode.LeastHasOneSuperUser,
                                                     "无法进行本次操作，请确保系统里面至少有一位管理员");
        }

        public Auth_User ValidateUser(string username, string password)
        {
            Auth_User user = _userRepository.GetUserByName(username);
            if(user!=null && user.IsEnabled)
            {
                string encryptPassword = EncryptUtils.EncodePassword(password, user.PasswordSalt);
                if (encryptPassword == user.Password)
                    return user;
            }

            return null;
        }

        public HashSet<string> GetUserPermissionKeys(int userId)
        {
            _userGroupsRepository.Include(t => t.Auth_Group.Auth_Group_Permissions);
            IList<Auth_User_Groups> groups = _userGroupsRepository.GetUserGroups(userId);
            HashSet<string> permissionKeys=new HashSet<string>();
            foreach (Auth_User_Groups group in groups)
            {
                foreach (Auth_Group_Permissions permission in group.Auth_Group.Auth_Group_Permissions)
                {
                    if (!permissionKeys.Contains(permission.PermissionKey))
                        permissionKeys.Add(permission.PermissionKey);
                }
            }

            return permissionKeys;
        }

        public void UpdateLoginDate(int id, DateTime loginDate)
        {
            var user = _userRepository.GetUser(id);
            user.LastLoginDate = loginDate;
            _userRepository.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            Auth_User user = _userRepository.GetUser(id);
            if(user.IsSuperUser)
            {
                ThrowWhenLessThanTwoSuperUser();
            }
            IList<Auth_User_Groups> userGroupses= _userGroupsRepository.GetUserGroups(id);
            foreach (Auth_User_Groups userGroups in userGroupses)
            {
                _userGroupsRepository.Remove(userGroups);
            }
            _userRepository.Remove(user);
        }
    }
}