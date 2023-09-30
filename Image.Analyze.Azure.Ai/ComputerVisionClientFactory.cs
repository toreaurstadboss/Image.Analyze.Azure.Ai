using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace Ocr.Handwriting.Azure.AI.Lib
{

    public interface IComputerVisionClientFactory
    {
        ComputerVisionClient CreateClient();
    }

    /// <summary>
    /// Client factory for Azure Cognitive Services - Computer vision.
    /// </summary>
    public class ComputerVisionClientFactory : IComputerVisionClientFactory
    {
        // Add your Computer Vision key and endpoint
        static string? _key = Environment.GetEnvironmentVariable("AZURE_COGNITIVE_SERVICES_VISION_KEY");
        static string? _endpoint = Environment.GetEnvironmentVariable("AZURE_COGNITIVE_SERVICES_VISION_ENDPOINT");

        public ComputerVisionClientFactory() : this(_key, _endpoint)
        {
        }

        public ComputerVisionClientFactory(string? key, string? endpoint)
        {
            _key = key;
            _endpoint = endpoint;
        }

        public ComputerVisionClient CreateClient()
        {
            if (_key == null)
            {
                throw new ArgumentNullException(_key, "The AZURE_COGNITIVE_SERVICES_VISION_KEY is not set. Set a system-level environment variable or provide this value by calling the overloaded constructor of this class.");
            }
            if (_endpoint == null)
            {
                throw new ArgumentNullException(_key, "The AZURE_COGNITIVE_SERVICES_VISION_ENDPOINT is not set. Set a system-level environment variable or provide this value by calling the overloaded constructor of this class.");
            }

            var client = Authenticate(_key!, _endpoint!);
            return client;
        }

        public static ComputerVisionClient Authenticate(string key, string endpoint) =>
            new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };

    }
}
