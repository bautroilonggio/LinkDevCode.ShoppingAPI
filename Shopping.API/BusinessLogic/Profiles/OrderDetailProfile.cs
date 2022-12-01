using AutoMapper;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(destination => destination.ProductName,
                           option => option.MapFrom(source => source.Product.Name));

            CreateMap<Cart, OrderDetail>();
        }
    }
}
