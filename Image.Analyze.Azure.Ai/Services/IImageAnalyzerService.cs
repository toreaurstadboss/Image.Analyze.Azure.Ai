using Azure.AI.Vision.ImageAnalysis;

namespace Image.Analyze.Azure.Ai.Lib
{
    public interface IImageAnalyzerService
    {
        Task<ImageAnalyzer> CreateImageAnalyzer(string imageFile);
    }
}