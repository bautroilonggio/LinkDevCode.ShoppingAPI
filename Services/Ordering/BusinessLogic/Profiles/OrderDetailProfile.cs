using AutoMapper;
using Ordering.DataAccess.Entities;
using Ordering.DataAccess.Models;

namespace Ordering.BusinessLogic.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailDto>();
        }
    }
}
