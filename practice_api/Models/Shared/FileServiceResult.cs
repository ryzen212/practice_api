namespace practice_api.Models.Shared
{
    public class FileServiceResult
    {
        public bool Error { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string Message { get; set; }

        public static FileServiceResult Success(string fileName, string filePath)
        {
            return new FileServiceResult
            {
                Error = false,
                FileName = fileName,
                FilePath = filePath
            };
        }

        public static FileServiceResult Failure(string message = "File upload failed")
        {
            return new FileServiceResult
            {
                Error = true,
                Message = message,

            };
        }
    }
}
