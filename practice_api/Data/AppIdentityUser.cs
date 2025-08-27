using Microsoft.AspNetCore.Identity;

namespace practice_api.Data
{
    public class AppIdentityUser : IdentityUser
    {
        public string? UserImg { get; set; }

        public ICollection<RefreshTokens> RefreshTokens { get; set; }
    }
}
