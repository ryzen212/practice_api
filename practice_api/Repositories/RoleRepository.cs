using Microsoft.AspNetCore.Identity;
using practice_api.Controllers;
using practice_api.Data;
using System.Data;

namespace practice_api.Repositories
{


    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<AppIdentityRole> _roleManager;

        public RoleRepository(RoleManager<AppIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public Task<IdentityResult> createAsync(AppIdentityRole role)
        {
          
         return _roleManager.CreateAsync(role);

        }
        public async Task<AppIdentityRole?> FindByNameAsync(string name) {
         
            return await _roleManager.FindByNameAsync(name);

        }
        public async Task<bool> RoleExistsAsync(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }



    }
}
