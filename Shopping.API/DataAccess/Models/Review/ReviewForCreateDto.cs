using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.API.DataAccess.Models
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
        public int Rating { get; set; }

        /// <summary>
        /// Content of review
        /// </summary>
        [MaxLength(255)]
        public string? Content { get; set; }

        /// <summary>
        /// Creation time of review
        /// </summary>
        [JsonIgnore]
        public DateOnly CreateAt { get; set; } = DateOnly.Parse(DateTime.Now.ToShortDateString());
    }
}
