using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rating.DataAccess.Entities
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RatingScore { get; set; }

        [MaxLength(255)]
        public string? Content { get; set; }

        public DateOnly CreateAt { get; set; } = DateOnly.Parse(DateTime.Now.ToShortDateString());

        public int ProductId { get; set; }

        public int UserId { get; set; }
    }
}
