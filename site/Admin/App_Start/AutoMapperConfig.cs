using Admin.Areas.Auth.Models;
using AutoMapper;
using Domain.InfoModel;
using Domain.Model;

namespace Admin
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Auth_User, EditUserViewModel>();
            Mapper.CreateMap<EditUserViewModel, Auth_UserProfile>();
            Mapper.CreateMap<Auth_UserProfile, Auth_User>();
        }

    }
}