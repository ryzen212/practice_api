using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using practice_api.Contracts;
using practice_api.Data;
using practice_api.Models.Users;
using System.Configuration;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace practice_api.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;


        public UserRepository(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
           

      
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

        public async Task<bool> EmailExistsAsync(string email, string? excludeId = null)
        {

            var query = _userManager.Users.AsQueryable()
                       .Where(u => u.Email == email);
            if (!string.IsNullOrEmpty(excludeId))
            {
                query = query.Where(u => u.Id != excludeId);
            }
      
            return await query.AnyAsync();
        }

        public async Task<bool> UserNameExistAsync(string username, string? excludeId = null)
        {
            var query = _userManager.Users.AsQueryable()
                      .Where(u => u.UserName == username);
            if (!string.IsNullOrEmpty(excludeId))
            {
                query = query.Where(u => u.Id != excludeId);
            }

            return await query.AnyAsync();
        }

        public async Task<IdentityResult> UpdateAsync( AppIdentityUser user)
        {

            return await _userManager.UpdateAsync(user);
        }
        public async Task<IdentityResult> RemoveFromRoleAsync(AppIdentityUser user, string role)
        {
          
            return await _userManager.RemoveFromRoleAsync(user, role);
        }


 
    }
}
