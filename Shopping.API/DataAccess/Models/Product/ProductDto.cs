using Shopping.API.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double SellingPrice { get; set; }

        public int TotalQuantity { get; set; }

        public DateOnly CreateAt { get; set; }

        public int NumberOfReview { get; set; }
    }
}
