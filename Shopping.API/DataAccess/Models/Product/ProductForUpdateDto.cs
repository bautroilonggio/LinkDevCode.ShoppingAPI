using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class ProductForUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public double Price { get; set; }

        public int TotalQuantity { get; set; }

        public ProductForUpdateDto(string name, string brand, string category)
        {
            Name = name;
            Brand = brand;
            Category = category;
        }
    }
}
