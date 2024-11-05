using Image.Analyze.Azure.Ai.Services;
using Ocr.Handwriting.Azure.AI.Services;
using TextCopy;

namespace Image.Analyze.Azure.Ai
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddScoped<IImageSaveService, ImageSaveService>();
            builder.Services.AddScoped<IImageAnalyzerService, ImageAnalyzerService>();
            builder.Services.InjectClipboard();
            return builder.Build();
        }
    }
}