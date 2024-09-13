using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiStylerApp.Models;
using MauiStylerApp.Services;
using MauiStylerApp.Tools;
using MauiStylerApp.Views;
using System.Collections.ObjectModel;

namespace MauiStylerApp.ViewModels;

[QueryProperty(nameof(CurrentTemplateId), nameof(CurrentTemplateId))]
[QueryProperty(nameof(IsEdit), nameof(IsEdit))]
public partial class PgStyleEditorViewModel : ObservableRecipient
{
    readonly IStyleTemplateService styleTemplateServ;
    readonly IDocumentService documentServ;

    public PgStyleEditorViewModel(IStyleTemplateService styleTemplateService, IDocumentService documentService, IColorsPalettesService colorsPalettesService)
    {
        styleTemplateServ = styleTemplateService;
        documentServ = documentService;
        colorsPalettesServ = colorsPalettesService;
        Palettes = [.. colorsPalettesServ.GetAll()];
        if (Palettes.Count > 0)
        {
            SelectedPaletteItem = Palettes[0];
        }
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
    ObservableCollection<ColorStyleGroup>? defaultColorStyle;

    [ObservableProperty]
    ColorStyle? selectedDefaultColorStyle;

    [ObservableProperty]
    ObservableCollection<ColorStyleGroup>? darkColorStyle;

    [ObservableProperty]
    ColorStyle? selectedDarkColorStyle;

    [ObservableProperty]
    bool hasDarkTheme;

    [RelayCommand]
    void SetEnableDisableDarkTheme()
    {
        HasDarkTheme = !HasDarkTheme;
    }

    [RelayCommand]
    async Task ShowNewColorStyleForSemanticColor()
    {
        IsActive = true;

        Dictionary<string, object> sendData = new()
        {
            { "SendToken", "H1I2J3K4-L5M6-N7O8-P9Q0-R1S2T3U4V5W6" },
            //{ "CurrentSeptionName", "CurrentSemanticColor" }
        };

        await Shell.Current.GoToAsync(nameof(PgEditColor), true, sendData);
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

        await Shell.Current.GoToAsync(nameof(PgEditColor), true, sendData);
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

        await Shell.Current.GoToAsync(nameof(PgEditColor), true, sendData);
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

        await Shell.Current.GoToAsync(nameof(PgEditColor), true, sendData);
    }

    [RelayCommand]
    async Task ShowEditColorStyle()
    {
        IsActive = true;

        var SelectedColorStyle = SelectedDefaultColorStyle is null ? SelectedDarkColorStyle : SelectedDefaultColorStyle;

        Dictionary<string, object> sendData = new()
        {
            { "SendToken", "A1B2C3D4-E5F6-7890-ABCD-EF1234567890" },
            //{ "CurrentSeptionName", CurrentSeptionName },
            { "CurrentColorStyle", SelectedColorStyle! }
        };

        await Shell.Current.GoToAsync(nameof(PgEditColor), true, sendData);
    }

    [RelayCommand]
    async Task RestoreAllColorsToDefaults()
    {
        LoadDefaultColorStyle();
        if (HasDarkTheme)
        {
            LoadDarkColorStyle();
        }
        await Task.CompletedTask;
    }

    #region PERSONALIZAR COLOR SELECCIONADO
    #region PALETAS DE COLORES
    readonly IColorsPalettesService colorsPalettesServ;

    [ObservableProperty]
    ObservableCollection<ColorPalette>? palettes;

    [ObservableProperty]
    ColorPalette? selectedPaletteItem;

    [ObservableProperty]
    ObservableCollection<Color>? colorsOfPalette;

    [ObservableProperty]
    Color? selectedColorOfPalette;

    [ObservableProperty]
    bool isVisibleInfo;

    [ObservableProperty]
    string? titleToolSelected = "Ecualizadores";

    [RelayCommand]
    void ShowEqualizers()
    {
        ColorsOfPalette = null;
    }

    [RelayCommand]
    async Task ShowPalettes()
    {
        var leng = SelectedPaletteItem!.ColorsList!.Count;
        if (leng > 42)
        {
            ColorsOfPalette = [.. SelectedPaletteItem!.ColorsList!.Take(42).Select(x => x.Value)];
        }
        else
        {
            ColorsOfPalette = [.. SelectedPaletteItem!.ColorsList!.Values];
        }
        await Task.CompletedTask;
    }

    partial void OnSelectedColorOfPaletteChanging(Color? value)
    {
        if (value is not null && !IsDefaultColor)
        {
            CurrentColor = NewColorSelected;
        }
    }

    partial void OnSelectedColorOfPaletteChanged(Color? value)
    {
        if (value is not null)
        {
            NewColorSelected = value;
        }
    }

    partial void OnColorsOfPaletteChanged(ObservableCollection<Color>? value)
    {
        TitleToolSelected = value is null ? "Ecualizadores" : "Paletas";
    }
    #endregion

    [ObservableProperty]
    Color? newColorSelected = Colors.White;

    [ObservableProperty]
    Color? currentColor = Colors.Black;

    [ObservableProperty]
    string? red;

    [ObservableProperty]
    string? green;

    [ObservableProperty]
    string? blue;

    [ObservableProperty]
    string? alpha;

    [ObservableProperty]
    string? hexadecimal;

    [ObservableProperty]
    Color? defaultColor;

    [ObservableProperty]
    bool isDefaultColor;

    [RelayCommand]
    void SetHexColor()
    {
        if (!string.IsNullOrEmpty(Hexadecimal) && Hexadecimal.Length >= 6)
        {
            string argbHex = Hexadecimal;
            if (Hexadecimal.Length == 8)
            {
                string a = Hexadecimal.Substring(6, 2);
                string rgb = Hexadecimal[..6];

                argbHex = a + rgb;
            }

            if (Color.TryParse("#" + argbHex, out Color vColor))
            {
                Red = (vColor.Red * 255).ToString();
                Green = (vColor.Green * 255).ToString();
                Blue = (vColor.Blue * 255).ToString();
                Alpha = (vColor.Alpha * 255).ToString();
                Hexadecimal = vColor.ToRgbaHex(true)[1..];
            }
        }
    }
    partial void OnDefaultColorChanged(Color? value)
    {
        if (value is not null)
        {
            IsDefaultColor = true;
        }
    }

    partial void OnIsDefaultColorChanged(bool value)
    {
        if (value)
        {
            CurrentColor = DefaultColor;
            //NewColorSelected = Colors.White;
        }
    }

    partial void OnNewColorSelectedChanged(Color? value)
    {
        if (value is not null)
        {
            Red = (value.Red * 255).ToString();
            Green = (value.Green * 255).ToString();
            Blue = (value.Blue * 255).ToString();
            Alpha = (value.Alpha * 255).ToString();
            Hexadecimal = value.ToRgbaHex(true)[1..];
        }
    }

    partial void OnSelectedPaletteItemChanged(ColorPalette? value)
    {
        if (value is not null && ColorsOfPalette is not null)
        {
            ColorsOfPalette = null;
            Task task = ShowPalettes();
        }
    }
    #endregion
    #endregion

    partial void OnIsEditChanged(string? value)
    {
        if (!string.IsNullOrEmpty(CurrentTemplateId) && !string.IsNullOrEmpty(value))
        {
            Title = bool.Parse(value) 
                ? $"Editar tema {CurrentTemplate!.Name}"
                : $"Nuevo tema basado en {CurrentTemplate!.Name}";
        }
    }

    partial void OnCurrentTemplateIdChanged(string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            CurrentTemplate = styleTemplateServ.GetById(new LiteDB.ObjectId(value));
            if (!string.IsNullOrEmpty(IsEdit))
            {
                Title = bool.Parse(IsEdit!) 
                    ? $"Editar tema {CurrentTemplate!.Name}"
                    : $"Nuevo tema basado en {CurrentTemplate!.Name}";
            }
        }
    }

    partial void OnIsVisibleStyleChanged(bool value)
    {
        bool checkColors = (DefaultColorStyle is null || DefaultColorStyle.Count == 0)
                && (DarkColorStyle is null || DarkColorStyle.Count == 0);
        if (!value && checkColors)
        {
            LoadColorStylesObservableCollections();
        }
    }

    partial void OnSelectedDefaultColorStyleChanged(ColorStyle? value)
    {
        if (value is not null)
        {
            DefaultColor = value.Value;
            SelectedDarkColorStyle = null;
        }
    }

    partial void OnSelectedDarkColorStyleChanged(ColorStyle? value)
    {
        if (value is not null)
        {
            DefaultColor = value.Value;
            SelectedDefaultColorStyle = null;
        }
    }

    partial void OnHasDarkThemeChanged(bool value)
    {
        if (value)
        {
            var darkColorThread = new Thread(LoadDarkColorStyle);
            darkColorThread.Start();
            darkColorThread.Join();
        }
        else
        {
            DarkColorStyle = null;
        }
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        // NewColorStyle
        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, ColorStyle, string>(this, "H1I2J3K4-L5M6-N7O8-P9Q0-R1S2T3U4V5W6", (r, m) =>
        {
            IsActive = false;

            SelectedDefaultColorStyle = null;
            SelectedDarkColorStyle = null;
        });

        // EditColorStyle
        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, ColorStyle, string>(this, "A1B2C3D4-E5F6-7890-ABCD-EF1234567890", (r, m) =>
        {
            IsActive = false;
            if (m.Scheme == ColorScheme.Light)
            {
                LoadDefaultColorStyle();
            }
            else
            {
                LoadDarkColorStyle();
            }

            SelectedDefaultColorStyle = null;
            SelectedDarkColorStyle = null;
        });

        WeakReferenceMessenger.Default.Register<PgStyleEditorViewModel, string, string>(this, "F4E5D6C7-B8A9-0B1C-D2E3-F4567890ABCD", (r, m) =>
        {
            if (m == "cancel")
            {
                SelectedDefaultColorStyle = null;
                SelectedDarkColorStyle = null;
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
        var defaultColorThread = new Thread(LoadDefaultColorStyle);

        //var darkColorThread = new Thread(LoadDarkColorStyle);

        defaultColorThread.Start();
        //darkColorThread.Start();

        defaultColorThread.Join();
        //darkColorThread.Join();
    }

    void LoadDefaultColorStyle()
    {
        if (CurrentTemplate is null)
        {
            var defaultTemplate = styleTemplateServ.GetFirst();
            if (defaultTemplate is not null)
            {
                List<ColorStyleGroup> groups = [];
                foreach (var item in defaultTemplate!.ColorStyles!.Where(x => x.Scheme == ColorScheme.Light).GroupBy(cs => cs.Tag))
                {
                    var s = item.ToArray();
                    var group = new ColorStyleGroup(item.Key!, item.ToArray());
                    groups.Add(group);
                }
                DefaultColorStyle = [.. groups];
            }
        }
        else
        {
            List<ColorStyleGroup> groups = [];
            foreach (var item in CurrentTemplate!.ColorStyles!.Where(x => x.Scheme == ColorScheme.Light).GroupBy(cs => cs.Tag))
            {
                var s = item.ToArray();
                var group = new ColorStyleGroup(item.Key!, item.ToArray());
                groups.Add(group);
            }
            DefaultColorStyle = [.. groups];
        }
    }

    void LoadDarkColorStyle()
    {
        if (CurrentTemplate is null)
        {
            var defaultTemplate = styleTemplateServ.GetFirst();
            List<ColorStyleGroup> groups = [];
            foreach (var item in defaultTemplate!.ColorStyles!.Where(x => x.Scheme == ColorScheme.Dark).GroupBy(cs => cs.Tag))
            {
                var s = item.ToArray();
                var group = new ColorStyleGroup(item.Key!, item.ToArray());
                groups.Add(group);
            }
            DarkColorStyle = [.. groups];
        }
        else
        {
            List<ColorStyleGroup> groups = [];
            foreach (var item in CurrentTemplate!.ColorStyles!.Where(x => x.Scheme == ColorScheme.Dark).GroupBy(cs => cs.Tag))
            {
                var s = item.ToArray();
                var group = new ColorStyleGroup(item.Key!, item.ToArray());
                groups.Add(group);
            }
            DarkColorStyle = [.. groups];
        }
    }
    #endregion
}
