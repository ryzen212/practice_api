using practice_api.Data;



namespace practice_api.Models.Auth
{
    public class ServiceResult
    {
        public bool Error { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }

        public Dictionary<string, string[]> Errors { get; set; } = new();



        public static ServiceResult Success(string status, string message)
        {
            return new ServiceResult
            {
                Error = false,
                Errors = null,
                Status = status,
                Message = message,
            };
        }

        public static ServiceResult Fail(string status, string message, Dictionary<string, string[]> errors)
        {
            return new ServiceResult
            {
                Error = true,
                Errors = errors,
                Status = status,
                Message = message,
            };
        }

    }
}
