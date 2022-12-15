using AutoMapper;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Models;

namespace Identity.BusinessLogic.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountForSignUpDto, Account>();
        }
    }
}
