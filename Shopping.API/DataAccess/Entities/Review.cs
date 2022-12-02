using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.API.DataAccess.Entities
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Rating { get; set; }

        [MaxLength(255)]
        public string? Content { get; set; }

        public DateOnly CreateAt { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("UserId")]
        public Account? User { get; set; }

        public int UserId { get; set; }

        public Review()
        {
            CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString());
        }
    }
}
