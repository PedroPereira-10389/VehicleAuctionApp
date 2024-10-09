using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using VehicleAuctionApp.Interfaces;
using VehicleAuctionApp.Services;

namespace VehicleAuctionApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                    fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                    fonts.AddFont("fa-v4compatibility-400.ttf", "FontAwesomeV4Compatibility");
                });
            builder.Services.AddSingleton<LoadingService>();
            builder.Services.AddTransient<IDialogService, DialogService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
