using Microsoft.AspNetCore.Identity.Data;
using practice_api.Models.Auth;
using practice_api.Models.Token;
using LoginRequest = practice_api.Models.Auth.LoginRequest;
using RegisterRequest = practice_api.Models.Auth.RegisterRequest;



namespace practice_api.Contracts
{
    public interface IAuthServices
    {

        public  Task<AuthServiceResult> Login(LoginRequest request);
        public Task<AuthServiceResult> Register(RegisterRequest request);

        public  Task<TokenServiceResult> Refresh(string refreshToken);

        public  Task<UserDto> GetUser(string userId);

    }
}
