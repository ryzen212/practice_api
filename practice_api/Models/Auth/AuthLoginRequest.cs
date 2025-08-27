using System.ComponentModel.DataAnnotations;

namespace practice_api.Models.Auth
{
    public class AuthLoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
