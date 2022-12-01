using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.API.DataAccess.Models.Product
{
    public class ProductForCreateDto
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

        [Required]
        [JsonIgnore]
        public DateOnly CreateAt { get; set; }

        public ProductForCreateDto(string name, string brand, string category)
        {
            Name = name;
            Brand = brand;
            Category = category;
            CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString());
        }
    }
}
