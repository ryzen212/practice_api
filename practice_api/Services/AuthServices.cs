using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_api.Contracts;
using practice_api.Data;
using practice_api.Models.Auth;
using practice_api.Models.Response;
using practice_api.Models.Token;

namespace practice_api.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenServices _tokenService;
        public AuthServices(IUserRepository userRepo, ITokenServices tokenService) {

            _userRepo = userRepo;
            _tokenService = tokenService;

        }
        public async Task<AuthServiceResult> Login(AuthLoginRequest request)
        {
            var user = await _userRepo.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return AuthServiceResult.Fail(null);

            }
            bool isPasswordValid = await _userRepo.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {

                return AuthServiceResult.Fail(null);
            }

            string accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateAndSaveRefreshToken(user);

            var response = AuthServiceResult.Success(accessToken, refreshToken.Token,user);
            return response;

        }

        public async Task<AuthServiceResult> Register(AuthRegister request)
        {

            var user = new AppIdentityUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userRepo.CreateAsync(user, request.Password);
            var roleResult = await _userRepo.AddToRoleAsync(user, request.Role);

            if (!result.Succeeded || !roleResult.Succeeded)
            {
                var errors = result.Errors.Any()
                  ? result.Errors.Select(e => e.Description).ToList()
                  : null;
                return AuthServiceResult.Fail(errors);

            }

            string accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateAndSaveRefreshToken(user);

            var response = AuthServiceResult.Success(accessToken, refreshToken.Token, user);
            return response;

        }


        public async Task<TokenServiceResult>Refresh(string refreshToken)
        {

            return await _tokenService.ValidateRefreshToken(refreshToken);
        }

        public async Task<UserDto> GetUser(string userId)
        {
           var user = await  _userRepo.FindByIdAsync(userId);
            return new UserDto
            {
                Id = user.Id,
              Avatar = user.Avatar,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        
    }
}
