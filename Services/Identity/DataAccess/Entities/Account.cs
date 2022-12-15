using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Identity.DataAccess.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Role { get; set; }

        [Required]
        public DateOnly CreatedAt { get; set; } = DateOnly.Parse(DateTime.Now.ToShortDateString());

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenCreatedAt { get; set; } = DateTime.Now;

        public DateTime RefreshTokenExpries { get; set; }
    }
}
