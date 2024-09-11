using MauiStylerApp.Resources.Localization;
using System.Globalization;

namespace MauiStylerApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        SetCulture();
        MainPage = new AppShell();
    }

    private void SetCulture()
    {
        var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        ResourceDictionary resourceDictionary;

        switch (culture)
        {
            case "en":
                resourceDictionary = new LanguageEn();
                break;
            default:
                return;
        }

        var defaultDictionary = Resources.MergedDictionaries.FirstOrDefault(d => d.Source?.ToString().Contains("LanguageEn.xaml") == true);
        if (defaultDictionary != null)
        {
            Resources.MergedDictionaries.Remove(defaultDictionary);
        }

        Resources.MergedDictionaries.Add(resourceDictionary);
    }
}
