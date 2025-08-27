using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using practice_api.Contracts;
using practice_api.Data;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace practice_api.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserRepository(UserManager<AppIdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;

      
        }

        public async Task<AppIdentityUser?> FindByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user; // may be null if user not found
        }

        public  Task<AppIdentityUser> FindByNameAsync(string userName)
        {
            var user = _userManager.FindByNameAsync(userName);

            return user;
        }
        public Task<bool> CheckPasswordAsync(AppIdentityUser user,string password)
        {
         
            return _userManager.CheckPasswordAsync(user, password);


        }
        public async Task<IdentityResult> CreateAsync(AppIdentityUser user, string password)
        {
  

            return await _userManager.CreateAsync(user, password);
        }

        public Task<IdentityResult> AddToRoleAsync(AppIdentityUser user , string role)
        {
         
            return _userManager.AddToRoleAsync(user, role);

     
        }

        public async Task<string> GetUserRole(AppIdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }



    }
}
