using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using practice_api.Contracts;
using practice_api.Data;
using practice_api.Models.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace practice_api.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _configuration;

        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepo;


        public TokenServices(IConfiguration configuration, AppDbContext context, IUserRepository userRepo)
        {

     
            _configuration = configuration;
            _context = context;
            _userRepo = userRepo;
        }


        public async Task<string> GenerateAccessToken(AppIdentityUser user)
        {
            string role = await _userRepo.GetUserRole(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        }

        public async Task<RefreshTokens> GenerateAndSaveRefreshToken(AppIdentityUser user)
        {
            var refreshToken = new RefreshTokens
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<TokenServiceResult> ValidateRefreshToken(string token)
        {
            RefreshTokens? refreshToken = await _context.RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r=>r.Token == token);

             if(refreshToken is null || refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new ApplicationException("Refreshtoken Expired");
            }
            string accessToken = await GenerateAccessToken(refreshToken.User);

            refreshToken.Token = GenerateRefreshToken();
            refreshToken.ExpiresAt = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
            return new TokenServiceResult { AccessToken = accessToken ,RefreshToken = refreshToken.Token};
        }

    }
}
