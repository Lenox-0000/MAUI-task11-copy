using CommunityToolkit.Maui;
using MAUITask11.ViewModels;
using Microsoft.Extensions.Logging;

namespace MAUITask11
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();
            builder.ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); });
            builder.UseMauiCommunityToolkit(options => { options.SetShouldEnableSnackbarOnWindows(true); });
            builder.Services.AddSingleton<EncodingViewModel>();
            builder.Services.AddSingleton<EncodingView>();
            //builder.Services.AddSingleton<DecodingViewModel>();
            builder.Services.AddSingleton<DecodingView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
