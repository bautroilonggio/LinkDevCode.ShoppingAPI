using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class UserForSignInDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public UserForSignInDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
