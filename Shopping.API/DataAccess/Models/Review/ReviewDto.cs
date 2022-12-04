using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    /// <summary>
    /// A DTO for review 
    /// </summary>
    public class ReviewDto
    {
        /// <summary>
        /// Id of review
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Rating for product
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Content of review
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Creation time of review
        /// </summary>
        public DateOnly CreateAt { get; set; }

        /// <summary>
        /// Name of author of the review
        /// </summary>
        public string? UserName { get; set; }
    }
}
