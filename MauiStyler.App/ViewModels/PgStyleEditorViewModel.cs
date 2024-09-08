using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiStyler.App.Models;
using MauiStyler.App.Services;
using MauiStyler.App.Tools;
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
    ObservableCollection<IGrouping<string, ColorStyle>>? defaultColorStyle;

    [ObservableProperty]
    ObservableCollection<IGrouping<string, ColorStyle>>? darkColorStyle;

    [ObservableProperty]
    ColorStyle? selectedColorStyle;

    [RelayCommand]
    async Task ShowNewColorStyleForSemanticColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "SendToken", "H1I2J3K4-L5M6-N7O8-P9Q0-R1S2T3U4V5W6" },
            //{ "CurrentSeptionName", "CurrentSemanticColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowNewColorStyleForSemanticDarkColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "SendToken", "H1I2J3K4-L5M6-N7O8-P9Q0-R1S2T3U4V5W6" },
            //{ "CurrentSeptionName", "CurrentSemanticDarkColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowNewColorStyleForNeutralColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "SendToken", "H1I2J3K4-L5M6-N7O8-P9Q0-R1S2T3U4V5W6" },
            //{ "CurrentSeptionName", "CurrentNeutralColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowNewColorStyleForNeutralDarkColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "SendToken", "H1I2J3K4-L5M6-N7O8-P9Q0-R1S2T3U4V5W6" },
            //{ "CurrentSeptionName", "CurrentNeutralDarkColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgNewEditItemColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowEditColorStyle()
    {
        IsActive = true;

        //var (CurrentSeptionName, CurrentColorStyle) = GetSelectedColorStyle();

        //if (CurrentColorStyle is null)
        //{
        //    return;
        //}

        Dictionary<string, object> sendData = new()
        {
            { "SendToken", "A1B2C3D4-E5F6-7890-ABCD-EF1234567890" },
            //{ "CurrentSeptionName", CurrentSeptionName },
            { "CurrentColorStyle", SelectedColorStyle! }
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
            bool checkColors = (DefaultColorStyle is null || DefaultColorStyle.Count == 0)
                && (DarkColorStyle is null || DarkColorStyle.Count == 0);
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

                LoadColorStylesObservableCollections();

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
        // NewColorStyle
        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, ColorStyle, string>(this, "H1I2J3K4-L5M6-N7O8-P9Q0-R1S2T3U4V5W6", (r, m) =>
        {
            IsActive = false;

            SelectedColorStyle = null;
        });

        // EditColorStyle
        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, ColorStyle, string>(this, "A1B2C3D4-E5F6-7890-ABCD-EF1234567890", (r, m) =>
        {
            IsActive = false;
            //var (currentSectionName, CurrentColorStyle) = GetSelectedColorStyle();

            //if (CurrentColorStyle is null || string.IsNullOrEmpty(currentSectionName))
            //{
            //    return;
            //}

            //ObservableCollection<ColorStyle>? targetCollection = currentSectionName switch
            //{
            //    "CurrentPrincipalColor" => PrincipalsColors,
            //    "CurrentPrincipalDarkColor" => PrincipalsDarkColors,
            //    "CurrentSemanticColor" => SemanticsColors,
            //    "CurrentSemanticDarkColor" => SemanticsDarkColors,
            //    "CurrentNeutralColor" => NeutralsColors,
            //    "CurrentNeutralDarkColor" => NeutralsDarkColors,
            //    _ => null
            //};

            //if (targetCollection is null)
            //{
            //    return;
            //}

            //int index = targetCollection.IndexOf(CurrentColorStyle);
            //if (index != -1)
            //{
            //    targetCollection[index] = m;

            //    SelectedColorStyle = m;

            //    OnPropertyChanged(nameof(targetCollection));
            //}
            //codigo bueno
            //int index = -1;
            //if (DefaultColorStyle!.IndexOf(SelectedColorStyle!) == index)
            //{
            //    index = DarkColorStyle!.IndexOf(SelectedColorStyle!);
            //    DarkColorStyle[index] = m;
            //}
            //else
            //{
            //    index = DefaultColorStyle!.IndexOf(SelectedColorStyle!);
            //    DefaultColorStyle[index] = m;
            //}

            if (m.Scheme == ColorScheme.Light)
            {
                LoadDefaultColorStyle();
            }
            else
            {
                LoadDarkColorStyle();
            }

            SelectedColorStyle = null;
        });

        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, string, string>(this, "F4E5D6C7-B8A9-0B1C-D2E3-F4567890ABCD", (r, m) =>
        {
            if (m == "cancel")
            {
                SelectedColorStyle = null;
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

    void LoadColorStylesObservableCollections()
    {
        var defaultColorThread = new Thread(() =>
        {
            LoadDefaultColorStyle();
        });

        var darkColorThread = new Thread(() =>
        {
            LoadDarkColorStyle();
        });

        defaultColorThread.Start();
        darkColorThread.Start();

        defaultColorThread.Join();
        darkColorThread.Join();
    }

    void LoadDefaultColorStyle()
    {
        if (CurrentTemplate is null)
        {
            var defaultTemplate = styleTemplateServ.GetFirst();
            if (defaultTemplate is not null)
            {
                DefaultColorStyle = [
                    .. defaultTemplate!.ColorStyles!
                            .Where(x => x.Scheme == ColorScheme.Light)
                            .GroupBy(cs => cs.Tag)
                ];
            }
        }
        else
        {
            DefaultColorStyle = [
                .. CurrentTemplate!.ColorStyles!
                        .Where(x => x.Scheme == ColorScheme.Light)
                        .GroupBy(cs => cs.Tag)
            ];
        }
    }

    void LoadDarkColorStyle()
    {
        if (CurrentTemplate is null)
        {
            var defaultTemplate = styleTemplateServ.GetFirst();
            if (defaultTemplate is not null)
            {
                DarkColorStyle = [
                    .. defaultTemplate!.ColorStyles!
                            .Where(x => x.Scheme == ColorScheme.Dark)
                            .GroupBy(cs => cs.Tag)
                ];
            }
        }
        else
        {
            DarkColorStyle = [
                .. CurrentTemplate!.ColorStyles!
                        .Where(x => x.Scheme == ColorScheme.Dark)
                        .GroupBy(cs => cs.Tag)
            ];
        }
    }

    //(string CurrentSectionName, ColorStyle? CurrentColorStyle) GetSelectedColorStyle()
    //{
    //    if (SelectedColorStyle is null)
    //    {
    //        return ("", null);
    //    }

    //    if (PrincipalsColors?.Contains(SelectedColorStyle) == true)
    //    {
    //        return ("CurrentPrincipalColor", SelectedColorStyle);
    //    }

    //    if (PrincipalsDarkColors?.Contains(SelectedColorStyle) == true)
    //    {
    //        return ("CurrentPrincipalDarkColor", SelectedColorStyle);
    //    }

    //    if (SemanticsColors?.Contains(SelectedColorStyle) == true)
    //    {
    //        return ("CurrentSemanticColor", SelectedColorStyle);
    //    }

    //    if (SemanticsDarkColors?.Contains(SelectedColorStyle) == true)
    //    {
    //        return ("CurrentSemanticDarkColor", SelectedColorStyle);
    //    }

    //    if (NeutralsColors?.Contains(SelectedColorStyle) == true)
    //    {
    //        return ("CurrentNeutralColor", SelectedColorStyle);
    //    }

    //    if (NeutralsDarkColors?.Contains(SelectedColorStyle) == true)
    //    {
    //        return ("CurrentNeutralDarkColor", SelectedColorStyle);
    //    }

    //    return ("", null);
    //}
    #endregion
}
