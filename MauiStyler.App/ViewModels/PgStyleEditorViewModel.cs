﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiStyler.App.Models;
using MauiStyler.App.Services;
using MauiStyler.App.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiStyler.App.ViewModels;

[QueryProperty(nameof(CurrentTemplateId), nameof(CurrentTemplateId))]
[QueryProperty(nameof(IsEdit), nameof(IsEdit))]
public partial class PgStyleEditorViewModel : ObservableRecipient
{
    readonly IStyleTemplateService styleTemplateServ;
    readonly IDocumentService documentServ;

    public PgStyleEditorViewModel(IStyleTemplateService styleTemplateService, IDocumentService documentService)
    {
        styleTemplateServ = styleTemplateService;
        documentServ = documentService;
    }

    [ObservableProperty]
    string? currentTemplateId;

    [ObservableProperty]
    StyleTemplate? currentTemplate;

    [ObservableProperty]
    string? isEdit;

    [ObservableProperty]
    ObservableCollection<string>? getAllViews;

    [ObservableProperty]
    string? title = "Nuevo tema";

    [ObservableProperty]
    bool isVisibleStyle = true;

    [RelayCommand]
    void SetVisibleStyle()
    {
        IsVisibleStyle = !IsVisibleStyle;
    }

    [RelayCommand]
    async Task GoToBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    #region COLORES
    [ObservableProperty]
    ObservableCollection<ItemColor>? principalsColors;

    [ObservableProperty]
    ObservableCollection<ItemColor>? semanticsColors;

    [ObservableProperty]
    ObservableCollection<ItemColor>? neutralsColors;

    [ObservableProperty]
    ObservableCollection<ItemColor>? principalsDarkColors;

    [ObservableProperty]
    ObservableCollection<ItemColor>? semanticsDarkColors;

    [ObservableProperty]
    ObservableCollection<ItemColor>? neutralsDarkColors;

    [ObservableProperty]
    ItemColor? selectedItemColor;

    [RelayCommand]
    async Task ShowNewItemColorForSemanticColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "CurrentSeptionName", "CurrentSemanticColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowNewItemColorForSemanticDarkColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "CurrentSeptionName", "CurrentSemanticDarkColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowNewItemColorForNeutralColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "CurrentSeptionName", "CurrentNeutralColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowNewItemColorForNeutralDarkColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "CurrentSeptionName", "CurrentNeutralDarkColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowEditItemColor()
    {
        IsActive = true;

        var (CurrentSeptionName, CurrentItemColor) = GetSelectedItemColor();

        if (CurrentItemColor is null)
        {
            return;
        }

        Dictionary<string, object> sendData = new()
        {
            { "CurrentSeptionName", CurrentSeptionName },
            { "CurrentItemColor", CurrentItemColor }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }
    #endregion

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(IsEdit))
        {
            if (!string.IsNullOrEmpty(CurrentTemplateId) && !string.IsNullOrEmpty(IsEdit))
            {
                Title = bool.Parse(IsEdit!) ?
                        $"Editar tema {CurrentTemplate!.Name}"
                        : $"Nuevo tema basado en {CurrentTemplate!.Name}";
            }
        }

        if (e.PropertyName == nameof(CurrentTemplateId))
        {
            if (!string.IsNullOrEmpty(CurrentTemplateId))
            {
                CurrentTemplate = styleTemplateServ.GetById(new LiteDB.ObjectId(CurrentTemplateId));
                if (!string.IsNullOrEmpty(IsEdit))
                {
                    Title = bool.Parse(IsEdit!) ?
                        $"Editar tema {CurrentTemplate!.Name}"
                        : $"Nuevo tema basado en {CurrentTemplate!.Name}";
                }
            }
        }

        if (e.PropertyName == nameof(IsVisibleStyle))
        {
            bool checkColors = (PrincipalsColors is null || PrincipalsColors.Count == 0)
                && (SemanticsColors is null || SemanticsColors.Count == 0)
                && (NeutralsColors is null || NeutralsColors.Count == 0)
                && (PrincipalsDarkColors is null || PrincipalsDarkColors.Count == 0)
                && (SemanticsDarkColors is null || SemanticsDarkColors.Count == 0)
                && (NeutralsDarkColors is null || NeutralsDarkColors.Count == 0);
            if (!IsVisibleStyle && checkColors)
            {
                //PrincipalsColors = [
                //    new ItemColor() { Name = "Primary", Value = Color.Parse("#512BD4") },
                //    new ItemColor() { Name = "Secondary", Value = Color.Parse("#2B0B98") },
                //    new ItemColor() { Name = "Accent", Value = Color.Parse("#2B0B98") }
                //];

                //SemanticsColors = [
                //    new ItemColor() { Name = "Error", Value = Color.Parse("#FF0000") },
                //    new ItemColor() { Name = "Success", Value = Color.Parse("#00FF00") },
                //    new ItemColor() { Name = "Warning", Value = Color.Parse("#FFFF00") }
                //];

                //NeutralsColors = [
                //    new ItemColor() { Name = "Gray250", Value = Color.Parse("#E1E1E1") },
                //    new ItemColor() { Name = "Gray500", Value = Color.Parse("#ACACAC") },
                //    new ItemColor() { Name = "Gray750", Value = Color.Parse("#6E6E6E") }
                //];
                //bool result = colorStyleServ.GenerateColorTemplate([.. PrincipalsColors], [.. SemanticsColors], [.. NeutralsColors]);
                if (CurrentTemplate is null)
                {
                    var defaultTemplate = styleTemplateServ.GetAll().First();

                    PrincipalsColors = [.. defaultTemplate.PrincipalStyle!.DefaultColorsStyle];
                    PrincipalsDarkColors = [.. defaultTemplate.PrincipalStyle!.DarkColorsStyle];

                    SemanticsColors = [.. defaultTemplate.SemanticStyle!.DefaultColorsStyle];
                    SemanticsDarkColors = [.. defaultTemplate.SemanticStyle!.DarkColorsStyle];

                    NeutralsColors = [.. defaultTemplate.NeutralStyle!.DefaultColorsStyle];
                    NeutralsDarkColors = [.. defaultTemplate.NeutralStyle!.DarkColorsStyle];
                }
                else
                {
                    PrincipalsColors = [.. CurrentTemplate.PrincipalStyle!.DefaultColorsStyle];
                    PrincipalsDarkColors = [.. CurrentTemplate.PrincipalStyle!.DarkColorsStyle];

                    SemanticsColors = [.. CurrentTemplate.SemanticStyle!.DefaultColorsStyle];
                    SemanticsDarkColors = [.. CurrentTemplate.SemanticStyle!.DarkColorsStyle];

                    NeutralsColors = [.. CurrentTemplate.NeutralStyle!.DefaultColorsStyle];
                    NeutralsDarkColors = [.. CurrentTemplate.NeutralStyle!.DarkColorsStyle];
                }

                //var sectionsColors = documentServ.LoadSelectedTemplate();

                //NeutralsColors = new(sectionsColors["NEUTRAL"]);
                //SemanticsColors = new(sectionsColors["SEMANTIC"]);
                //PrincipalsColors = new(sectionsColors["PRINCIPAL"]);

                //NeutralsDarkColors = new(sectionsColors["NEUTRAL"]);
                //SemanticsDarkColors = new(sectionsColors["SEMANTIC"]);
                //PrincipalsDarkColors = new(sectionsColors["PRINCIPAL"]);
            }
        }
    }

    protected override void OnActivated()
    {
        base.OnActivated();

        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, ItemColor, string>(this, "NewItemColor", (r, m) =>
        {
            IsActive = false;

            SelectedItemColor = null;
        });

        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, ItemColor, string>(this, "EditItemColor", (r, m) =>
        {
            IsActive = false;
            var (currentSectionName, currentItemColor) = GetSelectedItemColor();

            if (currentItemColor is null || string.IsNullOrEmpty(currentSectionName))
            {
                return;
            }

            ObservableCollection<ItemColor>? targetCollection = currentSectionName switch
            {
                "CurrentPrincipalColor" => PrincipalsColors,
                "CurrentPrincipalDarkColor" => PrincipalsDarkColors,
                "CurrentSemanticColor" => SemanticsColors,
                "CurrentSemanticDarkColor" => SemanticsDarkColors,
                "CurrentNeutralColor" => NeutralsColors,
                "CurrentNeutralDarkColor" => NeutralsDarkColors,
                _ => null
            };

            if (targetCollection is null)
            {
                return;
            }

            int index = targetCollection.IndexOf(currentItemColor);
            if (index != -1)
            {
                targetCollection[index] = m;

                SelectedItemColor = m;

                OnPropertyChanged(nameof(targetCollection));
            }

            SelectedItemColor = null;
        });

        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, string, string>(this, "F4E5D6C7-B8A9-0B1C-D2E3-F4567890ABCD", (r, m) =>
        {
            if (m == "cancel")
            {
                SelectedItemColor = null;
            }
            IsActive = false;
        });
    }

    #region EXTRA
    public async void InitializerPropertyAsync()
    {
        var assembly = typeof(View).Assembly;
        var types = await Task.Run(() => assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(View)) && t.Namespace == "Microsoft.Maui.Controls")
            .Select(t => t.Name));

        GetAllViews = [.. types];
    }


    (string CurrentSectionName, ItemColor? CurrentItemColor) GetSelectedItemColor()
    {
        if (SelectedItemColor is null)
        {
            return ("", null);
        }

        if (PrincipalsColors?.Contains(SelectedItemColor) == true)
        {
            return ("CurrentPrincipalColor", SelectedItemColor);
        }

        if (PrincipalsDarkColors?.Contains(SelectedItemColor) == true)
        {
            return ("CurrentPrincipalDarkColor", SelectedItemColor);
        }

        if (SemanticsColors?.Contains(SelectedItemColor) == true)
        {
            return ("CurrentSemanticColor", SelectedItemColor);
        }

        if (SemanticsDarkColors?.Contains(SelectedItemColor) == true)
        {
            return ("CurrentSemanticDarkColor", SelectedItemColor);
        }

        if (NeutralsColors?.Contains(SelectedItemColor) == true)
        {
            return ("CurrentNeutralColor", SelectedItemColor);
        }

        if (NeutralsDarkColors?.Contains(SelectedItemColor) == true)
        {
            return ("CurrentNeutralDarkColor", SelectedItemColor);
        }

        return ("", null);
    }
    #endregion
}
