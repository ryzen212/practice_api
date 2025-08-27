using Microsoft.AspNetCore.Identity;
using practice_api.Data;

namespace practice_api.Repositories
{


    public class RoleRepository
    {
        private readonly RoleManager<AppIdentityRole> _roleManager;

        public Task<IdentityResult> createAsync(AppIdentityRole role)
        {
         return _roleManager.CreateAsync(role);

        }
       
    }
}
