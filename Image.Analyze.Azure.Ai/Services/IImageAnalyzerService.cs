using Azure.AI.Vision.ImageAnalysis;

namespace Image.Analyze.Azure.Ai.Services
{
    public interface IImageAnalyzerService
    {
        Task<ImageAnalyzer?> CreateImageAnalyzer(string imageFile);
    }
}