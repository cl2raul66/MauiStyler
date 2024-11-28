using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiteDB;
using MauiStylerApp.Models;
using MauiStylerApp.Services;
using MauiStylerApp.Tools;
using MauiStylerApp.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiStylerApp.ViewModels;

public partial class PgThemesViewModel : ObservableRecipient
{
    readonly IStyleTemplateService styleTemplateServ;
    readonly IDocumentService documentServ;

    Dictionary<string, string> VMTokens = new()
    {
        { "TokenCancel", "901E87E2-856D-4DEB-BD16-B9102B0366AE" },
        { "TokenNewTemplate", "21FC9122-7466-4EC1-8E4E-0945BA952C76" },
        { "TokenEditTemplate", "7FC8EDE9-B0FF-4106-9C30-C2D6A440E990" },
        { "TokenNewTemplateBasedSelected", "DA3C5EC6-B78B-40E5-8E9B-10077DCC1B3C" }
    };

    public PgThemesViewModel(IStyleTemplateService styleTemplateService, IDocumentService documentService)
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
    async Task GoToMain()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    async Task GoToNewTemplate()
    {
        IsActive = true;
        Dictionary<string, object> sendData = new()
        {
            { nameof(VMTokens), VMTokensForSend() }
        };
        await Shell.Current.GoToAsync(nameof(PgStyleEditor), true, sendData);
    }

    [RelayCommand]
    async Task GoToEditTemplate()
    {
        IsActive = true;
        Dictionary<string, object> sendData = new()
        {
            { nameof(VMTokens), VMTokensForSend() },
            { "IsEdit", IsEdit.ToString() },
            { "CurrentTemplateId", SelectedTemplate!.Id! }
        };
        await Shell.Current.GoToAsync(nameof(PgStyleEditor), true, sendData);
    }

    [RelayCommand]
    async Task GoToNewTemplateBasedSelected()
    {
        IsActive = true;
        Dictionary<string, object> sendData = new()
        {
            { nameof(VMTokens), VMTokensForSend() },
            { "IsEdit", IsEdit.ToString() },
            { "CurrentTemplateId", SelectedTemplate!.Id! }
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

    protected override void OnActivated()
    {
        base.OnActivated();

        WeakReferenceMessenger.Default.Register<PgThemesViewModel, string, string>(this, VMTokens["TokenCancel"].ToString(), (r, m) =>
        {
            IsActive = false;
            if (bool.Parse(m))
            {

            }
        });

        WeakReferenceMessenger.Default.Register<PgThemesViewModel, StyleTemplate, string>(this, VMTokens["TokenNewTemplate"].ToString(), (r, m) =>
        {
            IsActive = false;
            var result = styleTemplateServ.Insert(m);
            if (string.IsNullOrEmpty(result))
            {
                GetStyleTemplates();
            }
        });

        WeakReferenceMessenger.Default.Register<PgThemesViewModel, StyleTemplate, string>(this, VMTokens["TokenNewTemplateBasedSelected"].ToString()!, (r, m) =>
        {
            IsActive = false;
            var result = styleTemplateServ.Insert(m);
            if (string.IsNullOrEmpty(result))
            {
                GetStyleTemplates();
            }
        });

        WeakReferenceMessenger.Default.Register<PgThemesViewModel, StyleTemplate, string>(this, VMTokens["TokenEditTemplate"].ToString(), (r, m) =>
        {
            IsActive = false;
            var result = styleTemplateServ.Update(m);
            if (result)
            {
                GetStyleTemplates();
            }
        });
    }

    #region EXTRA
    void GetStyleTemplates()
    {
        if (!styleTemplateServ.Exist)
        {
            List<ColorStyle> colorsStyle =
            [
                // PRINCIPAL
                new() { Name = "PrimaryCl", Value = Color.Parse("#FF512BD4"), Tag="PRINCIPAL", Scheme = ColorScheme.Light },
                new() { Name = "SecondaryCl", Value = Color.Parse("#FF2B0B98"), Tag="PRINCIPAL", Scheme = ColorScheme.Light },
                new() { Name = "AccentCl", Value = Color.Parse("#FF2D6FCC"), Tag="PRINCIPAL", Scheme = ColorScheme.Light },
                new() { Name = "PrimaryDarkCl", Value = Color.Parse("#FF512BD4"), Tag="PRINCIPAL", Scheme = ColorScheme.Dark },
                new() { Name = "SecondaryDarkCl", Value = Color.Parse("#FF2B0B98"), Tag="PRINCIPAL", Scheme = ColorScheme.Dark },
                new() { Name = "AccentDarkCl", Value = Color.Parse("#FF2D6FCC"), Tag="PRINCIPAL", Scheme = ColorScheme.Dark },
                // SEMANTIC
                new() { Name = "ErrorCl", Value = Color.Parse("#FFFF0000"), Tag="SEMANTIC", Scheme = ColorScheme.Light },
                new() { Name = "SuccessCl", Value = Color.Parse("#FF00FF00"), Tag="SEMANTIC", Scheme = ColorScheme.Light },
                new() { Name = "WarningCl", Value = Color.Parse("#FFFFFF00"), Tag="SEMANTIC", Scheme = ColorScheme.Light },
                new() { Name = "ErrorDarkCl", Value = Color.Parse("#FFFF0000"), Tag="SEMANTIC", Scheme = ColorScheme.Dark },
                new() { Name = "SuccessDarkCl", Value = Color.Parse("#FF00FF00"), Tag="SEMANTIC", Scheme = ColorScheme.Dark },
                new() { Name = "WarningDarkCl", Value = Color.Parse("#FFFFFF00"), Tag="SEMANTIC", Scheme = ColorScheme.Dark },            
                // NEUTRAL
                new() { Name = "ForegroundCl", Value = Color.Parse("#FFF7F5FF"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "BackgroundCl", Value = Color.Parse("#FF23135E"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "Gray250Cl", Value = Color.Parse("#FFE1E1E1"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "Gray500Cl", Value = Color.Parse("#FFACACAC"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "Gray750Cl", Value = Color.Parse("#FF6E6E6E"), Tag="NEUTRAL", Scheme = ColorScheme.Light },
                new() { Name = "ForegroundDarkCl", Value = Color.Parse("#FFF7F5FF"), Tag="NEUTRAL", Scheme = ColorScheme.Dark },
                new() { Name = "BackgroundDarkCl", Value = Color.Parse("#FF23135E"), Tag="NEUTRAL", Scheme = ColorScheme.Dark },
                new() { Name = "Gray250DarkCl", Value = Color.Parse("#FFE1E1E1"), Tag="NEUTRAL", Scheme = ColorScheme.Dark },
                new() { Name = "Gray500DarkCl", Value = Color.Parse("#FFACACAC"), Tag="NEUTRAL", Scheme = ColorScheme.Dark },
                new() { Name = "Gray750DarkCl", Value = Color.Parse("#FF6E6E6E"), Tag="NEUTRAL", Scheme = ColorScheme.Dark }
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

    string[] VMTokensForSend() => [.. VMTokens.Select(x => $"{x.Key}:{x.Value}")];
    #endregion
}
