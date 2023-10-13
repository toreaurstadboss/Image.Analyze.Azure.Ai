using Image.Analyze.Azure.Ai.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Ocr.Handwriting.Azure.AI.Services
{
  
    public interface IImageSaveService
    {

        Task<ImageSaveModel> SaveImage(IBrowserFile browserFile);

    }

}