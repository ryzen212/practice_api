using System.ComponentModel.DataAnnotations;

namespace practice_api.Models.Request
{
    public class AuthLoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
