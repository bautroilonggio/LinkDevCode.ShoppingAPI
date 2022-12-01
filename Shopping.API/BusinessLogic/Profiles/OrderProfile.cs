using AutoMapper;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderForCreateDto, Order>();
            CreateMap<OrderForUpdateDto, Order>();
        }
    }
}
