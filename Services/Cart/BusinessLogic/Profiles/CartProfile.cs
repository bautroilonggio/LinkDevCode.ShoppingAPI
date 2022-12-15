using AutoMapper;
using Cart.DataAccess.Entities;
using Cart.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart.DataAccess.Entities.Cart, CartDto>();

            CreateMap<CartForCreateDto, Cart.DataAccess.Entities.Cart>();
            CreateMap<CartForUpdateDto, Cart.DataAccess.Entities.Cart>();
        }
    }
}
