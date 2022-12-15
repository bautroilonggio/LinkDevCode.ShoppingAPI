using AutoMapper;
using Rating.DataAccess.Entities;
using Rating.DataAccess.Models;

namespace Rating.BusinessLogic.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewForCreateDto, Review>();
            CreateMap<ReviewForUpdateDto, Review>();
        }
    }
}
