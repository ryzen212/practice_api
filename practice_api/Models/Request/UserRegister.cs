using System.ComponentModel.DataAnnotations;

namespace practice_api.Models.Request
{
    public class UserRegister
    {
        [Required]
        [EmailAddress]

        public string UserName { get; set; }
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        [Required]
        public string Role { get; set; }

    }
}
