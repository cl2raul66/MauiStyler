using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStyler.App.Models;
using MauiStyler.App.Services;
using MauiStyler.App.Tools;
using MauiStyler.App.Views;
using System.Collections.ObjectModel;

namespace MauiStyler.App.ViewModels;

public partial class PgMainViewModel : ObservableObject
{
    readonly IStyleTemplateService styleTemplateServ;

    public PgMainViewModel(IStyleTemplateService styleTemplateService)
    {
        styleTemplateServ = styleTemplateService;
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

    #region EXTRA
    public void GetStyleTemplates()
    {
        if (!styleTemplateServ.Exist)
        {
            List<ItemColor> principalsColorsStyle = [];
            List<ItemColor> semanticsColorsStyle = [];
            List<ItemColor> neutralsColorsStyle = [];

            // PRINCIPAL
            principalsColorsStyle.Add(new() { Name = "Primary", Value = Color.Parse("#FF512BD4") });
            principalsColorsStyle.Add(new() { Name = "Secondary", Value = Color.Parse("#FF2B0B98") });
            principalsColorsStyle.Add(new() { Name = "Accent", Value = Color.Parse("#FF2B0B98") });

            PrincipalStyle ps = new()
            {
                DefaultColorsStyle = [.. principalsColorsStyle],
                DarkColorsStyle = [.. principalsColorsStyle]
            };

            // SEMANTIC
            semanticsColorsStyle.Add(new() { Name = "Error", Value = Color.Parse("#FFFF0000") });
            semanticsColorsStyle.Add(new() { Name = "Success", Value = Color.Parse("#FF00FF00") });
            semanticsColorsStyle.Add(new() { Name = "Warning", Value = Color.Parse("#FFFFFF00") });

            SemanticStyle ss = new()
            {
                DefaultColorsStyle = [.. semanticsColorsStyle],
                DarkColorsStyle = [.. semanticsColorsStyle]
            };

            // NEUTRAL
            neutralsColorsStyle.Add(new() { Name = "Foreground", Value = Color.Parse("#FFF7F5FF") });
            neutralsColorsStyle.Add(new() { Name = "Background", Value = Color.Parse("#FF23135E") });
            neutralsColorsStyle.Add(new() { Name = "Gray250", Value = Color.Parse("#FFE1E1E1") });
            neutralsColorsStyle.Add(new() { Name = "Gray500", Value = Color.Parse("#FFACACAC") });
            neutralsColorsStyle.Add(new() { Name = "Gray750", Value = Color.Parse("#FF6E6E6E") });

            NeutralStyle ns = new()
            {
                DefaultColorsStyle = [.. neutralsColorsStyle],
                DarkColorsStyle = [.. neutralsColorsStyle]
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
