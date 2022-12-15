using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.DataAccess.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Brand { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public double? SellingPrice { get; set; }

        public int TotalQuantity { get; set; }

        [Required]
        public DateOnly CreateAt { get; set; } = DateOnly.Parse(DateTime.Now.ToShortDateString());
    }
}
