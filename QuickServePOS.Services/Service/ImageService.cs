using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QuickServePOS.Services.IService;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace QuickServePOS.Services.Service
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(
            IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(
            IFormFile file,
            string folderName)
        {
            var uploadsFolder =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName =
                Guid.NewGuid() + ".jpg";

            var fullPath =
                Path.Combine(
                    uploadsFolder,
                    fileName);

            using var image =
                await Image.LoadAsync(
                    file.OpenReadStream());

            // Resize
            image.Mutate(x =>
                x.Resize(new ResizeOptions
                {
                    Size = new Size(500, 500),
                    Mode = ResizeMode.Max
                }));

            await image.SaveAsJpegAsync(
                fullPath,
                new JpegEncoder
                {
                    Quality = 90
                });

            return $"/uploads/{folderName}/{fileName}";
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var fullPath =
                Path.Combine(
                    _environment.WebRootPath,
                    imageUrl.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}