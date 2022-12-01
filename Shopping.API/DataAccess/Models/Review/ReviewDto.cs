using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string? Content { get; set; }

        public DateOnly CreateAt { get; set; }

        public string? UserName { get; set; }
    }
}
