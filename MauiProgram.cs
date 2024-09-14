using Microsoft.Extensions.Logging;
using RadialMaui.Interfaces;
using RadialMaui.ViewModels;
using RadialMaui.Platforms;
using RadialMaui.Util;

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
            builder.Services.AddHttpClient();

            builder.Services.AddTransient<GCodeControllerView>();
            builder.Services.AddTransient<ImageControllerView>();
            builder.Services.AddTransient<SVGControllerView>();

            builder.Services.AddTransient<GCodeControllerViewModel>();
            builder.Services.AddTransient<ImageControllerViewModel>();
            builder.Services.AddTransient<SVGControllerViewModel>();

            builder.Services.AddTransient<IFileSaveService, FileSaveService>();
            builder.Services.AddTransient<IFileUtil, FileUtil>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
