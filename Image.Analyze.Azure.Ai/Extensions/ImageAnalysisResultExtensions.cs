using Azure.AI.Vision.ImageAnalysis;
using System.Text;

namespace Image.Analyze.Azure.Ai.Extensions
{
    public static class ImageAnalysisResultExtensions
    {

        public static string GetBoundingBoxesJson(this ImageAnalysisResult result)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"[");

            int objectIndex = 0;
            foreach (var detectedObject in result.Objects)
            {
                sb.Append($"{{ \"Name\": \"{detectedObject.Name}\", \"Y\": {detectedObject.BoundingBox.Y}, \"X\": {detectedObject.BoundingBox.X}, \"Height\": {detectedObject.BoundingBox.Height}, \"Width\": {detectedObject.BoundingBox.Width}, \"Confidence\": \"{detectedObject.Confidence:0.0000}\" }}");
                objectIndex++;
                if (objectIndex < result.Objects?.Count)
                {
                    sb.Append($",{Environment.NewLine}");
                }
                else
                {
                    sb.Append($"{Environment.NewLine}");
                }
            }
            sb.Remove(sb.Length - 2, 1); //remove trailing comma at the end
            sb.AppendLine(@"]");
            return sb.ToString();
        }

        public static string OutputImageAnalysisResult(this ImageAnalysisResult result)
        {
            var sb = new StringBuilder();

            if (result.Reason == ImageAnalysisResultReason.Analyzed)
            {

                sb.AppendLine($" Image height = {result.ImageHeight}");
                sb.AppendLine($" Image width = {result.ImageWidth}");
                sb.AppendLine($" Model version = {result.ModelVersion}");

                if (result.Caption != null)
                {
                    sb.AppendLine(" Caption:");
                    sb.AppendLine($"   \"{result.Caption.Content}\", Confidence {result.Caption.Confidence:0.0000}");
                }

                if (result.DenseCaptions != null)
                {
                    sb.AppendLine(" Dense Captions:");
                    foreach (var caption in result.DenseCaptions)
                    {
                        sb.AppendLine($"   \"{caption.Content}\", Bounding box {caption.BoundingBox}, Confidence {caption.Confidence:0.0000}");
                    }
                }

                if (result.Objects != null)
                {
                    sb.AppendLine(" Objects:");
                    foreach (var detectedObject in result.Objects)
                    {
                        sb.AppendLine($"   \"{detectedObject.Name}\", Bounding box {detectedObject.BoundingBox}, Confidence {detectedObject.Confidence:0.0000}");
                    }
                }

                if (result.Tags != null)
                {
                    sb.AppendLine($" Tags:");
                    foreach (var tag in result.Tags)
                    {
                        sb.AppendLine($"   \"{tag.Name}\", Confidence {tag.Confidence:0.0000}");
                    }
                }

                if (result.People != null)
                {
                    sb.AppendLine($" People:");
                    foreach (var person in result.People)
                    {
                        sb.AppendLine($"   Bounding box {person.BoundingBox}, Confidence {person.Confidence:0.0000}");
                    }
                }

                if (result.CropSuggestions != null)
                {
                    sb.AppendLine($" Crop Suggestions:");
                    foreach (var cropSuggestion in result.CropSuggestions)
                    {
                        sb.AppendLine($"   Aspect ratio {cropSuggestion.AspectRatio}: "
                            + $"Crop suggestion {cropSuggestion.BoundingBox}");
                    };
                }

                if (result.Text != null)
                {
                    sb.AppendLine($" Text:");
                    foreach (var line in result.Text.Lines)
                    {
                        string pointsToString = "{" + string.Join(',', line.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
                        sb.AppendLine($"   Line: '{line.Content}', Bounding polygon {pointsToString}");

                        foreach (var word in line.Words)
                        {
                            pointsToString = "{" + string.Join(',', word.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
                            sb.AppendLine($"     Word: '{word.Content}', Bounding polygon {pointsToString}, Confidence {word.Confidence:0.0000}");
                        }
                    }
                }

                var resultDetails = ImageAnalysisResultDetails.FromResult(result);
                sb.AppendLine($" Result details:");
                sb.AppendLine($"   Image ID = {resultDetails.ImageId}");
                sb.AppendLine($"   Result ID = {resultDetails.ResultId}");
                sb.AppendLine($"   Connection URL = {resultDetails.ConnectionUrl}");
                sb.AppendLine($"   JSON result = {resultDetails.JsonResult}");
            }
            else
            {
                var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
                sb.AppendLine(" Analysis failed.");
                sb.AppendLine($"   Error reason : {errorDetails.Reason}");
                sb.AppendLine($"   Error code : {errorDetails.ErrorCode}");
                sb.AppendLine($"   Error message: {errorDetails.Message}");
            }

            return sb.ToString();
        }

    }
}
