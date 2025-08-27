using System.ComponentModel.DataAnnotations;

namespace practice_api.Models.Users
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]

        public string UserName { get; set; }


        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }


        [Required]
        public string Role { get; set; }
    }
}
