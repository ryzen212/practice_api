using practice_api.Data;


namespace practice_api.Models.Auth
{
    public class AuthServiceResult
    {
        public bool Error { get; set; }

        public Dictionary<string, string[]> Errors { get; set; }

        public UserDto User { get; set; }
        public string Message { get; set; }
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
                Avatar = user.Avatar,
                    UserName = user.UserName,
                    Email = user.Email
                },
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public static AuthServiceResult FailWithErrors(Dictionary<string, string[]> errors)
        {
            return new AuthServiceResult
            {
                Error = true,
                Errors = errors,
         
            };
        }
        public static AuthServiceResult Fail(string message)
        {
            return new AuthServiceResult
            {
                Error = true,
                Errors = null,
                Message = message,
       
            };
        }


    }
    }
