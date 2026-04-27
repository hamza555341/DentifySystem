using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class AttachmentService : IAttachmentService 
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long MaxAllowedFileSize = 5 * 1024 * 1024;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public async Task<string?> UploadAsync(string folderName, IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(folderName) || file.Length == 0) return null;
                if (file.Length > MaxAllowedFileSize) return null;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!AllowedExtensions.Contains(extension)) return null;

                var rootPath = _webHost.WebRootPath
                    ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                var folderPath = Path.Combine(rootPath, "images", folderName);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return $"/images/{folderName}/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload failed: {ex.Message}");
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                    return false;

                var rootPath = _webHost.WebRootPath
                    ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                var fullPath = Path.Combine(rootPath, "images", folderName, fileName);

                if (!File.Exists(fullPath)) return false;

                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
                return false;
            }
        }
    }
}
