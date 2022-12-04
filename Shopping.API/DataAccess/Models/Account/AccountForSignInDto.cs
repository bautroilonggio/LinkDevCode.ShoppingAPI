using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    /// <summary>
    /// A DTO for an account sign in
    /// </summary>
    public class AccountForSignInDto
    {
        /// <summary>
        /// Enter username for sign in
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        /// <summary>
        /// Enter password for sign in
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        /// <summary>
        /// Constructor of AccountForSignInDto
        /// </summary>
        /// <param name="userName">The username of account</param>
        /// <param name="password">The password of account</param>
        public AccountForSignInDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
