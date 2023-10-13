using Image.Analyze.Azure.Ai.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Ocr.Handwriting.Azure.AI.Services
{

    public class ImageSaveService : IImageSaveService
    {

        public async Task<ImageSaveModel> SaveImage(IBrowserFile browserFile)
        {
            var buffers = new byte[browserFile.Size];
            var bytes = await browserFile.OpenReadStream(maxAllowedSize: 30 * 1024 * 1024).ReadAsync(buffers);
            string imageType = browserFile.ContentType;

            var basePath = FileSystem.Current.AppDataDirectory;
            var imageSaveModel = new ImageSaveModel
            {
                SavedFilePath = Path.Combine(basePath, $"{Guid.NewGuid().ToString("N")}-{browserFile.Name}"),
                PreviewImageUrl = $"data:{imageType};base64,{Convert.ToBase64String(buffers)}",
                FilePath = browserFile.Name,
                FileSize = bytes / 1024,
            };

            await File.WriteAllBytesAsync(imageSaveModel.SavedFilePath, buffers);

            return imageSaveModel;
        }

    }
}