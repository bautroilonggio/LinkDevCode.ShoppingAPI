using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class AccountForSignUpDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [MaxLength(10)]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [MaxLength(10)]
        public string Role { get; set; }

        public AccountForSignUpDto(string userName, string password,
            string firstName, string lastName, DateOnly dateOfBirth,
            string phone, string email, string address, string role)
        {
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Phone = phone;
            Email = email;
            Address = address;
            Role = role;
        }
    }
}
