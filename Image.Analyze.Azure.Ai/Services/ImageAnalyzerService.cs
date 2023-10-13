using Azure;
using Azure.AI.Vision.Common;
using Azure.AI.Vision.ImageAnalysis;

namespace Image.Analyze.Azure.Ai.Lib
{

    public class ImageAnalyzerService : IImageAnalyzerService
    {

        public async Task<ImageAnalyzer> CreateImageAnalyzer(string imageFile)
        {
            string key = Environment.GetEnvironmentVariable("AZURE_COGNITIVE_SERVICES_VISION_SECONDARY_KEY");
            string endpoint = Environment.GetEnvironmentVariable("AZURE_COGNITIVE_SERVICES_VISION_SECONDARY_ENDPOINT");
            var visionServiceOptions = new VisionServiceOptions(new Uri(endpoint), new AzureKeyCredential(key));

            using VisionSource visionSource = CreateVisionSource(imageFile);

            var analysisOptions = CreateImageAnalysisOptions();

            var analyzer = new ImageAnalyzer(visionServiceOptions, visionSource, analysisOptions);
            return analyzer;

        }

        private static VisionSource CreateVisionSource(string imageFile)
        {
            using var stream = File.OpenRead(imageFile);
            using var reader = new StreamReader(stream);
            byte[] imageBuffer;
            using (var streamReader = new MemoryStream())
            {
                stream.CopyTo(streamReader);
                imageBuffer = streamReader.ToArray();
            }

            using var imageSourceBuffer = new ImageSourceBuffer();
            imageSourceBuffer.GetWriter().Write(imageBuffer);
            return VisionSource.FromImageSourceBuffer(imageSourceBuffer);
        }

        private static ImageAnalysisOptions CreateImageAnalysisOptions() => new ImageAnalysisOptions
        {
            Language = "en",
            GenderNeutralCaption = false,
            Features =
              ImageAnalysisFeature.CropSuggestions
            | ImageAnalysisFeature.Caption
            | ImageAnalysisFeature.DenseCaptions
            | ImageAnalysisFeature.Objects
            | ImageAnalysisFeature.People
            | ImageAnalysisFeature.Text
            | ImageAnalysisFeature.Tags
        };

    }

}