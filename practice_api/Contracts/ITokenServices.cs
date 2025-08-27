using practice_api.Data;
using practice_api.Models.Token;

namespace practice_api.Contracts
{
    public interface ITokenServices
    {

        public  Task<string> GenerateAccessToken(AppIdentityUser user);
        public string GenerateRefreshToken();

        public Task<RefreshTokens> GenerateAndSaveRefreshToken(AppIdentityUser user);
        public Task<TokenServiceResult> ValidateRefreshToken(string RefreshToken);

    }
}
