using practice_api.Models.Auth;
using practice_api.Models.Token;

namespace practice_api.Contracts
{
    public interface IAuthServices
    {

        public  Task<AuthServiceResult> Login(AuthLoginRequest request);
        public Task<AuthServiceResult> Register(AuthRegister request);

        public  Task<TokenServiceResult> Refresh(string refreshToken);

        public  Task<UserDto> GetUser(string userId);

    }
}
