using AutoMapper;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>()
                .ForMember(destination => destination.ProductName,
                           option => option.MapFrom(source => source.Product.Name))
                .ForMember(destination => destination.UserName,
                           option => option.MapFrom(source => source.User.UserName));

            CreateMap<CartForCreateDto, Cart>();
            CreateMap<CartForUpdateDto, Cart>();
        }
    }
}
