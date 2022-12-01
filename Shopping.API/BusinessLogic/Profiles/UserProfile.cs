using AutoMapper;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForSignUpDto, User>();
        }
    }
}
