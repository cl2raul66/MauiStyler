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
    ObservableCollection<ColorStyleGroup>? defaultColorStyle;

    [ObservableProperty]
    ColorStyle? selectedDefaultColorStyle;

    [ObservableProperty]
    ObservableCollection<ColorStyleGroup>? darkColorStyle;

    [ObservableProperty]
    ColorStyle? selectedDarkColorStyle;

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
                LoadColorStylesObservableCollections();
            }
        }

        if (e.PropertyName == nameof(SelectedDefaultColorStyle))
        {
            if (SelectedDefaultColorStyle is not null)
            {
                SelectedDarkColorStyle = null;
            }
        }

        if (e.PropertyName == nameof(SelectedDarkColorStyle))
        {
            if (SelectedDarkColorStyle is not null)
            {
                SelectedDefaultColorStyle = null;
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
            foreach (var item in defaultTemplate!.ColorStyles!.Where(x => x.Scheme == ColorScheme.Light).GroupBy(cs => cs.Tag))
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
            foreach (var item in CurrentTemplate!.ColorStyles!.Where(x => x.Scheme == ColorScheme.Light).GroupBy(cs => cs.Tag))
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
