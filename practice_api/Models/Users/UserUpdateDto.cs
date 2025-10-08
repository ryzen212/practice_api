

namespace practice_api.Models.Users
{
    public class UserUpdateDto
    {
        public string Id {  get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }


        public string PhoneNumber { get; set; }


        public string Role { get; set; }
        public bool AvatarChanged { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
