using CommunityToolkit.Maui;
using MAUITask11.Services;
using MAUITask11.ViewModels;
using Microsoft.Extensions.Logging;

namespace MAUITask11
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
                    fonts.AddFont("OpenSans-Regular.ttf",   "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf",  "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit(options =>
                {
                    // Enables toast/snackbar notifications on Windows
                    options.SetShouldEnableSnackbarOnWindows(true);
                });

            // ── Services (singleton — one instance for the whole app lifetime) ──
            builder.Services.AddSingleton<PeselService>();

            // ── ViewModels ──
            builder.Services.AddSingleton<DecodingViewModel>();
            builder.Services.AddSingleton<EncodingViewModel>();

            // ── Views ──
            builder.Services.AddSingleton<DecodingView>();
            builder.Services.AddSingleton<EncodingView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
