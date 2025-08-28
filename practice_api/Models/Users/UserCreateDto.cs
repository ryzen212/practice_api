using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace practice_api.Models.Users
{
    public class UserCreateDto
    {


        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}
