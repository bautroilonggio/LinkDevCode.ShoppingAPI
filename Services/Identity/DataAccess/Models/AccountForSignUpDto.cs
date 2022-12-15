using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Identity.DataAccess.Models
{
    /// <summary>
    /// A DTO for an account sign up
    /// </summary>
    public class AccountForSignUpDto
    {
        /// <summary>
        /// Username of new account
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? UserName { get; set; }

        /// <summary>
        /// Password of new account
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        /// <summary>
        /// FirstName of new account
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        /// <summary>
        /// LastName of new account
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? LastName { get; set; }

        /// <summary>
        /// Birthday of new account
        /// </summary>
        [Required]
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// Phone number of new account
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string? Phone { get; set; }

        /// <summary>
        /// Email of new account
        /// </summary>
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// Address of new account
        /// </summary>
        [Required]
        public string? Address { get; set; }

        /// <summary>
        /// Role of new account
        /// </summary>
        [JsonIgnore]
        public string? Role { get; set; } = "USER";
    }
}
