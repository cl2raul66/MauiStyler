using CommunityToolkit.Maui;
using MauiStylerApp.Services;
using MauiStylerApp.ViewModels;
using MauiStylerApp.Views;
using Microsoft.Extensions.Logging;

namespace MauiStylerApp;

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

        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources/Localization");

        builder.Services.AddSingleton<IDocumentService, DocumentService>();
        builder.Services.AddSingleton<IColorsPalettesService, ColorsPalettesService>();
        builder.Services.AddSingleton<IStyleTemplateService, StyleTemplateService>();
        builder.Services.AddSingleton<IStyleableComponentsService, StyleableComponentsService>();
        builder.Services.AddSingleton<ITargetTypeService, TargetTypeService>();

        builder.Services.AddTransient<PgMain, PgMainViewModel>();
        builder.Services.AddTransient<PgSettings, PgSettingsViewModel>();
        builder.Services.AddTransient<PgThemes, PgThemesViewModel>();
        builder.Services.AddTransient<PgStyleEditor, PgStyleEditorViewModel>();
        builder.Services.AddTransient<PgEditColor, PgEditColorViewModel>();
        builder.Services.AddTransient<PgLayouts, PgLayoutsViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
