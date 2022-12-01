using AutoMapper;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;
using Shopping.API.DataAccess.Models.Product;

namespace Shopping.API.BusinessLogic.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(destination => destination.NumberOfReview,
                           options => options.MapFrom(source => source.Reviews.Count));
            CreateMap<Product, ProductDetailDto>();
            CreateMap<ProductForCreateDto, Product>();
            CreateMap<ProductForUpdateDto, Product>();
        }
    }
}
