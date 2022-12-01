using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class ReviewForUpdateDto
    {
        [Required]
        public int Rating { get; set; }

        [MaxLength(255)]
        public string? Content { get; set; }
    }
}
