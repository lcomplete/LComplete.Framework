using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exception;
using Domain.Model;
using Domain.Repositories;
using LComplete.Framework.Exceptions;

namespace Services.Impl
{
    public class Auth_GroupService:IAuth_GroupService
    {
        private IAuth_GroupRepository _groupRepository;
        private IAuth_Group_PermissionsRepository _permissionsRepository;

        public Auth_GroupService(IAuth_GroupRepository groupRepository,IAuth_Group_PermissionsRepository permissionsRepository)
        {
            _groupRepository = groupRepository;
            _permissionsRepository = permissionsRepository;
        }

        public IList<Auth_Group> GetGroups()
        {
            return _groupRepository.ToList();
        }

        public Auth_Group GetGroup(int id)
        {
            _groupRepository.Include(t => t.Auth_Group_Permissions);
            return _groupRepository.GetGroup(id);
        }

        public void Save(Auth_Group authGroup)
        {
            Auth_Group existsGroup = _groupRepository.GetGroupByName(authGroup.Name);
            if(existsGroup!=null && existsGroup.Id!=authGroup.Id)
                throw new DataAlreadyExistsException("分组名已经存在");

            if (authGroup.Id == 0)
            {
                authGroup.CreateDate = DateTime.Now;
                _groupRepository.Add(authGroup);
            }
            else
            {
                IList<Auth_Group_Permissions> permissionses =
                    _permissionsRepository.GetPermissionsByGroupId(authGroup.Id);
                foreach (var permission in permissionses)
                {
                    var newPermission =
                        authGroup.Auth_Group_Permissions.FirstOrDefault(t => t.PermissionKey == permission.PermissionKey);
                    if(newPermission!=null)
                    {
                        authGroup.Auth_Group_Permissions.Remove(newPermission);//权限已存在 从待更新列表中删除
                    }
                    else
                    {
                        _permissionsRepository.Remove(permission);
                    }
                }
                foreach (var newPermission in authGroup.Auth_Group_Permissions)
                {
                    _permissionsRepository.Add(newPermission);
                }

                if (existsGroup == null)
                {
                    existsGroup = _groupRepository.GetGroup(authGroup.Id);
                }
                existsGroup.Name = authGroup.Name;
                _groupRepository.SaveChanges();
            }
        }

        public int Delete(int id)
        {
            _groupRepository.Include(t => t.Auth_User_Groups);
            Auth_Group group = _groupRepository.GetGroup(id);
            int result = 0;
            if(group!=null)
            {
                if(group.Auth_User_Groups.Count==0)
                {
                    _permissionsRepository.DeleteByGroupId(id);
                    _groupRepository.Remove(group);
                    result= 1;
                }
                else
                    result= -1;
            }

            return result;
        }
    }
}