using Microsoft.AspNetCore.Components.WebView.Maui;
using Image.Analyze.Azure.Ai.Data;
using TextCopy;
using Ocr.Handwriting.Azure.AI.Services;

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

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddScoped<IImageSaveService, ImageSaveService>();
            builder.Services.InjectClipboard();
            return builder.Build();
        }
    }
}