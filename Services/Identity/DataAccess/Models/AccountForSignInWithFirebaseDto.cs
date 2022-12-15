using System.ComponentModel.DataAnnotations;

namespace Identity.DataAccess.Models
{
    /// <summary>
    /// A DTO for an account sign in with firebase
    /// </summary>
    public class AccountForSignInWithFirebaseDto
    {
        /// <summary>
        /// Enter username for sign in
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// Enter password for sign in
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }
    }
}
