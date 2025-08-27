using Microsoft.AspNetCore.Identity;
using practice_api.Data;

namespace practice_api.Contracts
{
    public interface IUserRepository
    {
        public Task<AppIdentityUser> FindByNameAsync(string userName);
        public Task<AppIdentityUser?> FindByIdAsync(string userId);
        public Task<bool> CheckPasswordAsync(AppIdentityUser user, string password);

        public  Task<IdentityResult> CreateAsync(AppIdentityUser user, string password);

        public Task<IdentityResult> AddToRoleAsync(AppIdentityUser user, string role);
        public  Task<string> GetUserRole(AppIdentityUser user);
    }
}
