using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;

namespace UbiSGBD
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
                    fonts.AddFont("opensans_semibold.ttf", "OpenSansSemiBold");
                    fonts.AddFont("fa_solid.ttf", "FontAwesome");
                    fonts.AddFont("fabmdl2.ttf", "Fabric");
                });


            ModifyEntry();

            return builder.Build();
        }

        public static void ModifyEntry()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoMoreBorders", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif IOS || MACCATALYST
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
#endif
            });
        }
    }
}