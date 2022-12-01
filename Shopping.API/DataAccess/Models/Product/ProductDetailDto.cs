using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class ProductDetailDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public int TotalQuantity { get; set; }

        public DateOnly CreateAt { get; set; }

        public int NumberOfReview
        {
            get
            {
                return Reviews.Count;
            }
        }

        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}
