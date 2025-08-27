using practice_api.Data;
using practice_api.Models.Response;


namespace practice_api.Models.Auth
{
    public class AuthServiceResult
    {
        public bool Error { get; set; }

        public List<string> Errors { get; set; }

        public UserDto User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public static AuthServiceResult Success(string accessToken, string refreshToken, AppIdentityUser user)
        {
            return new AuthServiceResult
            {
                Error = false,
                Errors = null,
                User = new UserDto
                {
                    Id = user.Id,
                    UserImg = user.UserImg,
                    UserName = user.UserName,
                    Email = user.Email
                },
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public static AuthServiceResult Fail(List<string> Errors)
        {
            return new AuthServiceResult
            {
                Error = true,
                Errors = Errors,
                User = null,
                AccessToken = null,
                RefreshToken = null
            };
        }


  
    }
    }
