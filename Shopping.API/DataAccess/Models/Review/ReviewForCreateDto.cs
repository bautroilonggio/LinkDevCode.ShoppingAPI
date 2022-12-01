using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.API.DataAccess.Models
{
    public class ReviewForCreateDto
    {
        [Required]
        public int Rating { get; set; }

        [MaxLength(255)]
        public string? Content { get; set; }

        [JsonIgnore]
        public DateOnly CreateAt { get; set; }

        public ReviewForCreateDto()
        {
            CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString());
        }
    }
}
