using CommunityToolkit.Maui;
using MauiStyler.App.Services;
using MauiStyler.App.ViewModels;
using MauiStyler.App.Views;
using Microsoft.Extensions.Logging;

namespace MauiStyler.App;

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
                fonts.AddFont("icofont.ttf", "icofont");
                fonts.AddFont("Iosevka-Regular.ttf", "iosevkaRegular");
                fonts.AddFont("NFCode-Regular.ttf", "nfcodeRegular");
            });

        builder.Services.AddSingleton<IColorStyleService, ColorStyleService>();
        builder.Services.AddSingleton<IStyleTemplateService, StyleTemplateService>();

        builder.Services.AddTransient<PgMain, PgMainViewModel>();
        builder.Services.AddTransient<PgStyleEditor, PgStyleEditorViewModel>();
        builder.Services.AddTransient<PgNewEditItemColor, PgNewEditItemColorViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
