using AutoMapper;
using Catalog.DataAccess.Entities;
using Catalog.DataAccess.Models;
using Catalog.DataAccess.Models.Product;

namespace Catalog.BusinessLogic.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductForCreateDto, Product>();
            CreateMap<ProductForUpdateDto, Product>();
        }
    }
}
