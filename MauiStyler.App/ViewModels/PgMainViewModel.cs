using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB;
using MauiStyler.App.Models;
using MauiStyler.App.Services;
using MauiStyler.App.Tools;
using MauiStyler.App.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiStyler.App.ViewModels;

public partial class PgMainViewModel : ObservableObject
{
    readonly IStyleTemplateService styleTemplateServ;
    readonly IDocumentService documentServ;

    public PgMainViewModel(IStyleTemplateService styleTemplateService, IDocumentService documentService)
    {
        styleTemplateServ = styleTemplateService;
        documentServ = documentService;
        GetStyleTemplates();
    }

    [ObservableProperty]
    ObservableCollection<TemplateItem>? styleTemplates = [];

    [ObservableProperty]
    TemplateItem? selectedTemplate;

    [ObservableProperty]
    bool isEdit;

    [RelayCommand]
    async Task GoToSetting()
    {
        await Shell.Current.GoToAsync(nameof(PgSettings), true);
    }

    [RelayCommand]
    async Task GoToNewTemplate()
    {
        await Shell.Current.GoToAsync(nameof(PgStyleEditor), true);
    }

    [RelayCommand]
    async Task GoToEditTemplate()
    {
        Dictionary<string, object> sendData = new()
        {
            { "IsEdit", IsEdit.ToString() },
            {"CurrentTemplateId", SelectedTemplate!.Id!}
        };
        await Shell.Current.GoToAsync(nameof(PgStyleEditor), true, sendData);
    }

    [RelayCommand]
    async Task GoToNewTemplateBasedSelected()
    {
        Dictionary<string, object> sendData = new()
        {
            { "IsEdit", IsEdit.ToString() },
            {"CurrentTemplateId", SelectedTemplate!.Id!}
        };
        await Shell.Current.GoToAsync(nameof(PgStyleEditor), true, sendData);
    }

    [RelayCommand]
    async Task Export()
    {
        var template = styleTemplateServ.GetById(new ObjectId(SelectedTemplate!.Id));
        if (template is null)
        {
            return;
        }
        var docs = documentServ.GenerateColorTemplate(template);
        await FileHelper.ExportTemplate([.. docs.Values]);
        await FileHelper.DeleteFilesAndDirectoriesAsync([.. docs.Values]);
        await Shell.Current.DisplayAlert("Mensaje", "Se exporto el tema satisfactoriamente.", "Cerrar");
        SelectedTemplate = null;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(SelectedTemplate))
        {
            if (SelectedTemplate is not null)
            {
                IsEdit = SelectedTemplate.IsCustomTemplate;
            }
        }
    }

    #region EXTRA
    public void GetStyleTemplates()
    {
        if (!styleTemplateServ.Exist)
        {
            List<ColorStyle> colorsStyle =
            [
                // PRINCIPAL
                new() { Name = "Primary", Value = Color.Parse("#FF512BD4"), Tag="PRINCIPAL", Scheme = ColorScheme.Light },
                new() { Name = "Secondary", Value = Color.Parse("#FF2B0B98"), Tag="PRINCIPAL", Scheme = ColorScheme.Light },
                new() { Name = "Accent", Value = Color.Parse("#FF2D6FCC"), Tag="PRINCIPAL", Scheme = ColorScheme.Light },
                new() { Name = "PrimaryDark", Value = Color.Parse("#FF512BD4"), Tag="PRINCIPAL", Scheme = ColorScheme.Dark },
                new() { Name = "SecondaryDark", Value = Color.Parse("#FF2B0B98"), Tag="PRINCIPAL", Scheme = ColorScheme.Dark },
                new() { Name = "AccentDark", Value = Color.Parse("#FF2D6FCC"), Tag="PRINCIPAL", Scheme = ColorScheme.Dark },
                // SEMANTIC
                new() { Name = "Error", Value = Color.Parse("#FFFF0000"), Tag="SEMANTIC", Scheme = ColorScheme.Light },
                new() { Name = "Success", Value = Color.Parse("#FF00FF00"), Tag="SEMANTIC", Scheme = ColorScheme.Light },
                new() { Name = "Warning", Value = Color.Parse("#FFFFFF00"), Tag="SEMANTIC", Scheme = ColorScheme.Light },
                new() { Name = "ErrorDark", Value = Color.Parse("#FFFF0000"), Tag="SEMANTIC", Scheme = ColorScheme.Dark },
                new() { Name = "SuccessDark", Value = Color.Parse("#FF00FF00"), Tag="SEMANTIC", Scheme = ColorScheme.Dark },
                new() { Name = "WarningDark", Value = Color.Parse("#FFFFFF00"), Tag="SEMANTIC", Scheme = ColorScheme.Dark },            
                // NEUTRAL
                new() { Name = "Foreground", Value = Color.Parse("#FFF7F5FF"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "Background", Value = Color.Parse("#FF23135E"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "Gray250", Value = Color.Parse("#FFE1E1E1"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "Gray500", Value = Color.Parse("#FFACACAC"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "Gray750", Value = Color.Parse("#FF6E6E6E"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "ForegroundDark", Value = Color.Parse("#FFF7F5FF"), Tag="NEUTRAL", Scheme = ColorScheme.Dark },
                new() { Name = "BackgroundDark", Value = Color.Parse("#FF23135E"), Tag="NEUTRAL", Scheme = ColorScheme.Dark },
                new() { Name = "Gray250Dark", Value = Color.Parse("#FFE1E1E1"), Tag="NEUTRAL", Scheme = ColorScheme.Dark },
                new() { Name = "Gray500Dark", Value = Color.Parse("#FFACACAC"), Tag="NEUTRAL", Scheme = ColorScheme.Dark },
                new() { Name = "Gray750Dark", Value = Color.Parse("#FF6E6E6E"), Tag="NEUTRAL", Scheme = ColorScheme.Dark }
            ];

            StyleTemplate mauiTemplate = new()
            {
                Name = "MAUI",
                Description = "Tema predeterminado.",
                IsCustomTemplate = false,
                ColorStyles = [.. colorsStyle]
            };

            _ = styleTemplateServ.Insert(mauiTemplate);
        }

        StyleTemplates = [.. styleTemplateServ.GetAll().Select(TemplateItemFactory.Create)];
    }
    #endregion
}
