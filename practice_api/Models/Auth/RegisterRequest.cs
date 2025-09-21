using System.ComponentModel.DataAnnotations;

namespace practice_api.Models.Auth
{
    public class RegisterRequest
    {
  
        public string UserName { get; set; }
        public string Password { get; set; }

     
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


       
        public string Role { get; set; }

    }
}
