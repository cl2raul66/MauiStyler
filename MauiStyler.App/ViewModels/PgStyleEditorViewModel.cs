using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStyler.App.Models;
using MauiStyler.App.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiStyler.App.ViewModels;

[QueryProperty(nameof(Title), "title")]
public partial class PgStyleEditorViewModel : ObservableObject
{
    readonly IColorStyleService colorStyleServ;

    public PgStyleEditorViewModel(IColorStyleService colorStyleService)
    {
        colorStyleServ = colorStyleService;

        InitializerProperty();
    }

    [ObservableProperty]
    ObservableCollection<string>? getAllViews;

    [ObservableProperty]
    string? title;

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
    ItemColor? selectedPrincipalColor;

    [ObservableProperty]
    ObservableCollection<ItemColor>? semanticsColors;

    [ObservableProperty]
    ItemColor? selectedSemanticColor;

    [ObservableProperty]
    ObservableCollection<ItemColor>? neutralsColors;

    [ObservableProperty]
    ItemColor? selectedNeutralColor;

    [ObservableProperty]
    ObservableCollection<ItemColor>? principalsDarkColors;

    [ObservableProperty]
    ItemColor? selectedPrincipalDarkColor;

    [ObservableProperty]
    ObservableCollection<ItemColor>? semanticsDarkColors;

    [ObservableProperty]
    ItemColor? selectedSemanticDarkColor;

    [ObservableProperty]
    ObservableCollection<ItemColor>? neutralsDarkColors;

    [ObservableProperty]
    ItemColor? selectedNeutralDarkColor;
    #endregion

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

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

                var sectionsColors = colorStyleServ.LoadDefaultTemplate();

                NeutralsColors = new(sectionsColors["NEUTRAL"]);
                SemanticsColors = new(sectionsColors["SEMANTIC"]);
                PrincipalsColors = new(sectionsColors["PRINCIPAL"]);

                NeutralsDarkColors = new(sectionsColors["NEUTRAL"]);
                SemanticsDarkColors = new(sectionsColors["SEMANTIC"]);
                PrincipalsDarkColors = new(sectionsColors["PRINCIPAL"]);
            }
        }

    }
    #region EXTRA

    void InitializerProperty()
    {
        var assembly = typeof(View).Assembly;
        var types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(View)) && t.Namespace == "Microsoft.Maui.Controls")
            .Select(t => t.Name);

        GetAllViews = [.. types];
    }

    #endregion
}
