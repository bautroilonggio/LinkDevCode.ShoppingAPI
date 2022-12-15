using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rating.DataAccess.Models
{
    /// <summary>
    /// A DTO for review to create
    /// </summary>
    public class ReviewForCreateDto
    {
        /// <summary>
        /// Rating for product
        /// </summary>
        [Required]
        public int RatingScore { get; set; }

        /// <summary>
        /// Content of review
        /// </summary>
        [MaxLength(255)]
        public string? Content { get; set; }
    }
}
