using AutoMapper;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>()
                .ForMember(destination => destination.UserName,
                           options => options.MapFrom(source => source.User.UserName));
            CreateMap<ReviewForCreateDto, Review>();
            CreateMap<ReviewForUpdateDto, Review>();
        }
    }
}
