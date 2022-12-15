using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Identity.DataAccess.Models
{
    /// <summary>
    /// A DTO for an account sign up with Firebase
    /// </summary>
    public class AccountForSignUpWithFirebaseDto
    {
        /// <summary>
        /// Username of new account
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// Password of new account
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        /// <summary>
        /// Password of new account
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Role of new account
        /// </summary>
        [JsonIgnore]
        public string? Role { get; set; } = "USER";
    }
}
