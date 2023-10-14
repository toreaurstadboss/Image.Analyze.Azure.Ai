using Azure.AI.Vision.ImageAnalysis;
using Image.Analyze.Azure.Ai.Extensions;
using Image.Analyze.Azure.Ai.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text;

namespace Image.Analyze.Azure.Ai.Pages
{
    partial class Index
    {

        private IndexModel Model = new();

        //https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/call-analyze-image-40?WT.mc_id=twitter&pivots=programming-language-csharp

        private string ImageInfo = string.Empty;

        private async Task Submit()
        {
            if (Model.PreviewImageUrl == null || Model.SavedFilePath == null)
            {
                await Application.Current.MainPage.DisplayAlert($"MAUI Blazor Image Analyzer App", $"You must select an image first before running Image Analysis. Supported formats are .jpeg, .jpg and .png", "Ok", "Cancel");
                return;
            }

            using var imageAnalyzer = await ImageAnalyzerService.CreateImageAnalyzer(Model.SavedFilePath);

            ImageAnalysisResult analysisResult = await imageAnalyzer.AnalyzeAsync();

            if (analysisResult.Reason == ImageAnalysisResultReason.Analyzed)
            {
                Model.ImageAnalysisOutputText = analysisResult.OutputImageAnalysisResult();
                var sb = new StringBuilder();
                sb.AppendLine(@"[");

                int objectIndex = 0;
                foreach (var detectedObject in analysisResult.Objects)
                {
                    sb.Append($"{{ \"Name\": \"{detectedObject.Name}\", \"Y\": {detectedObject.BoundingBox.Y}, \"X\": {detectedObject.BoundingBox.X}, \"Height\": {detectedObject.BoundingBox.Height}, \"Width\": {detectedObject.BoundingBox.Width}, \"Confidence\": \"{detectedObject.Confidence:0.0000}\" }}");
                    objectIndex++;
                    if (objectIndex < analysisResult.Objects?.Count)
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
                await JsRunTime.InvokeVoidAsync("LoadBoundingBoxes", sb.ToString());

            }
            else
            {
                ImageInfo = $"The image analysis did not perform its analysis. Reason: {analysisResult.Reason}";
            }


            StateHasChanged(); //visual refresh here
        }

        private async Task CopyTextToClipboard()
        {
            await Clipboard.SetTextAsync(Model.ImageAnalysisOutputText);
            await Application.Current.MainPage.DisplayAlert($"MAUI Blazor Image Analyzer App", $"The copied text was put into the clipboard. Character length: {Model.ImageAnalysisOutputText?.Length}", "Ok", "Cancel");
        }

        private async Task OnInputFile(InputFileChangeEventArgs args)
        {
            var imageSaveModel = await ImageSaveService.SaveImage(args.File);
            Model = new IndexModel(imageSaveModel);
            await Application.Current.MainPage.DisplayAlert($"MAUI Blazor ImageAnalyzer app App", $"Wrote file to location : {Model.SavedFilePath} Size is: {Model.FileSize} kB", "Ok", "Cancel");
        }


    }
}

