namespace practice_api.Models.Response
{
    public class AuthResponse
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public static AuthResponse Success(string accessToken, string refreshToken, string message = "Operation succeeded")
        {
            return new AuthResponse
            {
                Error = false,
                Status = "Success",
                Message = message,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public static AuthResponse Fail(string message = "Operation failed")
        {
            return new AuthResponse
            {
                Error = true,
                Status = "Fail",
                Message = message,
                AccessToken = null,
                RefreshToken = null
            };
        }
    }
}