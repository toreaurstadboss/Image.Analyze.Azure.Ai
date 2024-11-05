using Azure.AI.Vision.ImageAnalysis;
using Image.Analyze.Azure.Ai.Extensions;
using Image.Analyze.Azure.Ai.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Image.Analyze.Azure.Ai.Pages
{
    partial class Index
    {

        private IndexModel Model = new();

        //https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/call-analyze-image-40?WT.mc_id=twitter&pivots=programming-language-csharp

        private string ImageInfo = string.Empty;

        private async Task Submit()
        {
            if (Model.SavedFilePath == null || Model.PreviewImageUrl == null || Model.SavedFilePath == null && Application.Current?.MainPage != null)
            {
                await Application.Current!.MainPage!.DisplayAlert($"MAUI Blazor Image Analyzer App", $"You must select an image first before running Image Analysis. Supported formats are .jpeg, .jpg and .png", "Ok", "Cancel");
                return;
            }

            using var imageAnalyzer = await ImageAnalyzerService.CreateImageAnalyzer(Model.SavedFilePath!);

            if (imageAnalyzer == null)
            {
                await Task.CompletedTask;
            }

            ImageAnalysisResult analysisResult = await imageAnalyzer!.AnalyzeAsync();

            if (analysisResult.Reason == ImageAnalysisResultReason.Analyzed)
            {
                Model.ImageAnalysisOutputText = analysisResult.OutputImageAnalysisResult();
                Model.Caption = $"{analysisResult.Caption.Content} Confidence: {analysisResult.Caption.Confidence:F2}";
                Model.Tags = analysisResult.Tags.Select(t => $"{t.Name} (Confidence: {t.Confidence:F2})").ToList();
                var jsonBboxes = analysisResult.GetBoundingBoxesJson();
                await JsRunTime.InvokeVoidAsync("LoadBoundingBoxes", jsonBboxes);
            }
            else
            {
                ImageInfo = $"The image analysis did not perform its analysis. Reason: {analysisResult.Reason}";
            }

            StateHasChanged(); //visual refresh here
        }

        private async Task CopyTextToClipboard()
        {
            if (Application.Current?.MainPage == null)
            {
                return;
            }
            await Clipboard.SetTextAsync(Model.ImageAnalysisOutputText);
            await Application.Current.MainPage.DisplayAlert($"MAUI Blazor Image Analyzer App", $"The copied text was put into the clipboard. Character length: {Model.ImageAnalysisOutputText?.Length}", "Ok", "Cancel");
        }

        private async Task OnInputFile(InputFileChangeEventArgs args)
        {
            if (Application.Current?.MainPage == null)
            {
                return;
            }
            var imageSaveModel = await ImageSaveService.SaveImage(args.File);
            Model = new IndexModel(imageSaveModel);
            await Application.Current.MainPage.DisplayAlert($"MAUI Blazor ImageAnalyzer app App", $"Wrote file to location : {Model.SavedFilePath} Size is: {Model.FileSize} kB", "Ok", "Cancel");
        }


    }
}

