using AutoMapper;
using Ordering.DataAccess.Entities;
using Ordering.DataAccess.Models;

namespace Ordering.BusinessLogic.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderIncludeProductsDto>();
            CreateMap<Order, OrderWithoutProductsDto>();
            CreateMap<OrderForCreateDto, Order>();
            CreateMap<OrderForUpdateDto, Order>();
        }
    }
}
