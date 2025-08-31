using Microsoft.AspNetCore.Identity;
using practice_api.Data;

namespace practice_api.Controllers
{
    public interface IRoleRepository
    {
        public Task<IdentityResult> createAsync(AppIdentityRole role);

        public  Task<AppIdentityRole?> FindByNameAsync(string name);

        public Task<bool> RoleExistsAsync(string name);


    }
}
