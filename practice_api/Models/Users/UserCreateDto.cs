
namespace practice_api.Models.Users
{
    public class UserCreateDto
    {

        public string UserName { get; set; }

   
        public string Email { get; set; }

   
        public string PhoneNumber { get; set; }

   
        public string Password { get; set; }

        public string Role { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
