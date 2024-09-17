using Microsoft.Extensions.Logging;
using RadialMaui.Interfaces;
using RadialMaui.ViewModels;
using RadialMaui.Platforms;
using RadialMaui.Services;
using RadialMaui.Views;

namespace RadialMaui
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<GCodeControllerView>();
            builder.Services.AddTransient<ImageControllerView>();
            builder.Services.AddTransient<SVGControllerView>();

            builder.Services.AddTransient<GCodeControllerViewModel>();
            builder.Services.AddTransient<ImageControllerViewModel>();
            builder.Services.AddTransient<SVGControllerViewModel>();

            builder.Services.AddHttpClient<GCodeControllerViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://kvash.tar.ge/GCode/");
            });

            builder.Services.AddHttpClient<ImageControllerViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://kvash.tar.ge/Image/");
            });

            builder.Services.AddHttpClient<SVGControllerViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://kvash.tar.ge/SVG/");
            });

            builder.Services.AddTransient<IFileSaveService, FileSaveService>();
            builder.Services.AddTransient<IFileService, FileService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
