using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB;
using MauiStyler.App.Models;
using MauiStyler.App.Services;
using MauiStyler.App.Tools;
using MauiStyler.App.Views;
using System.Collections.ObjectModel;
using System.Xml.Linq;

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

    [RelayCommand]
    async Task GoToPgStyleEditor()
    {
        Dictionary<string, object> sendData = new()
        {
            {"title", "Nuevo"}
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
        var docs = documentServ.GenerateColorTemplate(template.PrincipalStyle!, template.SemanticStyle!, template.NeutralStyle!);
        await FileHelper.ExportTemplate([.. docs.Values]);
        await FileHelper.DeleteFilesAndDirectoriesAsync([.. docs.Values]);
        await Shell.Current.DisplayAlert("Mensaje", "Se exporto el tema satisfactoriamente.", "Cerrar");
        SelectedTemplate = null;
    }

    #region EXTRA
    public void GetStyleTemplates()
    {
        if (!styleTemplateServ.Exist)
        {
            // PRINCIPAL
            List<ItemColor> principalsColorsStyle =
            [
                new() { Name = "Primary", Value = Color.Parse("#FF512BD4") },
                new() { Name = "Secondary", Value = Color.Parse("#FF2B0B98") },
                new() { Name = "Accent", Value = Color.Parse("#FF2B0B98") }
            ];
            List<ItemColor> principalsDarkColorsStyle =
            [
                new() { Name = "PrimaryDark", Value = Color.Parse("#FF512BD4") },
                new() { Name = "SecondaryDark", Value = Color.Parse("#FF2B0B98") },
                new() { Name = "AccentDark", Value = Color.Parse("#FF2B0B98") }
            ];

            PrincipalStyle ps = new()
            {
                DefaultColorsStyle = [.. principalsColorsStyle],
                DarkColorsStyle = [.. principalsDarkColorsStyle]
            };

            // SEMANTIC
            List<ItemColor> semanticsColorsStyle =
            [
                new() { Name = "Error", Value = Color.Parse("#FFFF0000") },
                new() { Name = "Success", Value = Color.Parse("#FF00FF00") },
                new() { Name = "Warning", Value = Color.Parse("#FFFFFF00") }
            ];
            List<ItemColor> semanticsDarkColorsStyle =
            [
                new() { Name = "ErrorDark", Value = Color.Parse("#FFFF0000") },
                new() { Name = "SuccessDark", Value = Color.Parse("#FF00FF00") },
                new() { Name = "WarningDark", Value = Color.Parse("#FFFFFF00") }
            ];

            SemanticStyle ss = new()
            {
                DefaultColorsStyle = [.. semanticsColorsStyle],
                DarkColorsStyle = [.. semanticsDarkColorsStyle]
            };

            // NEUTRAL
            List<ItemColor> neutralsColorsStyle =
            [
                new() { Name = "Foreground", Value = Color.Parse("#FFF7F5FF") },
                new() { Name = "Background", Value = Color.Parse("#FF23135E") },
                new() { Name = "Gray250", Value = Color.Parse("#FFE1E1E1") },
                new() { Name = "Gray500", Value = Color.Parse("#FFACACAC") },
                new() { Name = "Gray750", Value = Color.Parse("#FF6E6E6E") }
            ];
            List<ItemColor> neutralsDarkColorsStyle =
            [
                new() { Name = "ForegroundDark", Value = Color.Parse("#FFF7F5FF") },
                new() { Name = "BackgroundDark", Value = Color.Parse("#FF23135E") },
                new() { Name = "Gray250Dark", Value = Color.Parse("#FFE1E1E1") },
                new() { Name = "Gray500Dark", Value = Color.Parse("#FFACACAC") },
                new() { Name = "Gray750Dark", Value = Color.Parse("#FF6E6E6E") }
            ];

            NeutralStyle ns = new()
            {
                DefaultColorsStyle = [.. neutralsColorsStyle],
                DarkColorsStyle = [.. neutralsDarkColorsStyle]
            };

            StyleTemplate mauiTemplate = new()
            {
                Name = "MAUI",
                Description = "Tema predeterminado.",
                IsCustomTemplate = false,
                PrincipalStyle = ps,
                SemanticStyle = ss,
                NeutralStyle = ns
            };

            _ = styleTemplateServ.Insert(mauiTemplate);
        }

        StyleTemplates = [.. styleTemplateServ.GetAll().Select(TemplateItemFactory.Create)];
    }
    #endregion
}
