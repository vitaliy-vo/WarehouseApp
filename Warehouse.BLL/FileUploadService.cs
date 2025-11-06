using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;

namespace Warehouse.BLL
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IBrowserFile file, string uploadSubfolder)
        {

            var safeFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.Name);

            var uploadDirectory = Path.Combine(_environment.WebRootPath, uploadSubfolder);
            if (!Directory.Exists(uploadDirectory))
                Directory.CreateDirectory(uploadDirectory);

            var fullPath = Path.Combine(uploadDirectory, safeFileName);

            await using var fs = new FileStream(fullPath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fs);

            return $"{uploadSubfolder}/{safeFileName}";
        }

        public async Task<string> UploadFileToRootAsync(IBrowserFile file)
        {
            // Дополнительная проверка размера
            const long maxFileSize = 2 * 1024 * 1024; // 2 MB
            if (file.Size > maxFileSize)
            {
                throw new InvalidOperationException($"File size exceeds the maximum allowed size of {maxFileSize} bytes.");
            }


            var safeFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.Name);


            var rootPath = _environment.WebRootPath;


            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            var fullPath = Path.Combine(rootPath, safeFileName);

            try
            {

                await using var fileStream = new FileStream(fullPath, FileMode.Create);
                await using var stream = file.OpenReadStream(maxFileSize);
                await stream.CopyToAsync(fileStream);

                return safeFileName;
            }
            catch (Exception ex)
            {

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                throw new Exception($"Failed to upload file: {ex.Message}", ex);
            }
        }

    }
}
