using practice_api.Models.Shared;

namespace practice_api.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _webHost;
        public FileService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }


        public FileServiceResult PrepareSaveFile(IFormFile file, string path)
        {
            if (file == null || file.Length == 0)
            {
                return FileServiceResult.Success(null, null);
            }

            string originalExtension = Path.GetExtension(file.FileName);
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string randomString = Path.GetRandomFileName().Replace(".", "").Substring(0, 8);
            string fileName = $"{timeStamp}_{randomString}{originalExtension}";
            string relativePath = $"/{path.Trim('/')}/{fileName}";

            return FileServiceResult.Success(fileName, relativePath);
        }

        // Uploads a file to the specified path on the server
        public async Task UploadAsync(IFormFile file, string fileName, string path)
        {

            if (file != null)
            {

                string uploadFolder = Path.Combine(_webHost.WebRootPath, path);

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string fileSavePath = Path.Combine(uploadFolder, fileName);

                using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

        }

        public bool DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return true;

            // Combine with wwwroot to get full physical path
            var fullPath = Path.Combine(_webHost.WebRootPath, relativePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;

            }

            return false;
        }

        public bool BulkDeleteFile(IList<string> relativePaths)
        {
            bool result = false;
            foreach (string relativePath in relativePaths)
            {
                if (this.DeleteFile(relativePath))
                {
                    result = true; // mark true only if at least one file is deleted
                }

            }
            return result;
        }

        public string ResolveUserImageUrl(string userImg)
        {

            if (string.IsNullOrWhiteSpace(userImg))
                return "/images/no_image.png";


            var fullPath = Path.Combine(_webHost.WebRootPath, userImg.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                return userImg;

            }

            return "/images/no_image.png";
        }
    }
}
