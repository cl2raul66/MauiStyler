using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiStyler.App.Models;
using MauiStyler.App.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MauiStyler.App.ViewModels;

[QueryProperty(nameof(CurrentSeptionName), nameof(CurrentSeptionName))]
[QueryProperty(nameof(CurrentItemColor), nameof(CurrentItemColor))]
public partial class PgNewEditItemColorViewModel : ObservableValidator
{
    readonly IColorsPalettesService colorsPalettesServ;

    public PgNewEditItemColorViewModel(IColorsPalettesService colorsPalettesService)
    {
        colorsPalettesServ = colorsPalettesService;
        Palettes = [.. colorsPalettesServ.GetAll()];
        if (Palettes is not null && Palettes.Count > 0)
        {
            SelectedPaletteItem = Palettes[0];
        }        
    }

    [ObservableProperty]
    string? currentSeptionName;

    [ObservableProperty]
    ItemColor? currentItemColor;

    [ObservableProperty]
    ObservableCollection<ColorPalette>? palettes;

    [ObservableProperty]
    ColorPalette? selectedPaletteItem;

    [ObservableProperty]
    ObservableCollection<Color>? colorsOfPalette;

    [ObservableProperty]
    Color? selectedColorOfPalette;

    [ObservableProperty]
    bool isLoadingColorsOfPalette;

    [ObservableProperty]
    Color? lastColorSelected = Colors.Black;

    [ObservableProperty]
    Color? currentColor = Colors.White;

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

    [ObservableProperty]
    string? title = "Nuevo";

    [ObservableProperty]
    [Required]
    [MinLength(2)]
    string? nameColor;

    [ObservableProperty]
    bool isVisibleInfo;

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

            SelectedPaletteItem = null;
        }
    }

    [RelayCommand]
    async Task Save()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            IsVisibleInfo = true;
            await Task.Delay(4000);
            IsVisibleInfo = false;
            return;
        }

        ItemColor itemColor = new()
        {
            Name = NameColor!.Trim().ToUpper(),
            Value = CurrentColor
        };

        string tokenSend = CurrentColor is null ? "NewItemColor" : "EditItemColor";

        _ = WeakReferenceMessenger.Default.Send(itemColor, tokenSend);

        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    async Task GoToBack()
    {
        _ = WeakReferenceMessenger.Default.Send("cancel", "F4E5D6C7-B8A9-0B1C-D2E3-F4567890ABCD");
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    private void SelectColor(Color color)
    {
        SelectedColorOfPalette = color;
    }

    partial void OnIsDefaultColorChanged(bool value)
    {
        if (value && CurrentItemColor is not null)
        {
            LastColorSelected = CurrentItemColor.Value;
        }
    }
    partial void OnCurrentItemColorChanged(ItemColor? value)
    {
        IsDefaultColor = value is not null;
        if (value is not null)
        {
            Title = "Modificar";
            NameColor = value.Name;
        }        
    }
    partial void OnSelectedPaletteItemChanged(ColorPalette? value)
    {
        if (value is null) return;

        IsLoadingColorsOfPalette = true;
        ColorsOfPalette = null;

        ColorsOfPalette = [.. value.ColorsList!.Values];

        IsLoadingColorsOfPalette = false;
    }
    partial void OnColorsOfPaletteChanged(ObservableCollection<Color>? value)
    {
        if (value is not null && value.Count > 0)
        {
            SelectedColorOfPalette = value[0];
        }
    }
    partial void OnSelectedColorOfPaletteChanging(Color? value)
    {
        if (value is not null && !IsDefaultColor)
        {
            LastColorSelected = CurrentColor;
        }
    }
    partial void OnSelectedColorOfPaletteChanged(Color? value)
    {
        if (value is not null)
        {
            Red = (value.Red * 255).ToString();
            Green = (value.Green * 255).ToString();
            Blue = (value.Blue * 255).ToString();
            Alpha = (value.Alpha * 255).ToString();
            Hexadecimal = value.ToRgbaHex(true)[1..];
            CurrentColor = value;
        }
    }
    //partial void OnRedChanged(string? value){
    //    if (!Color.TryParse(value, out Color color)) return;
    //    Red = Math.Round(color.Red * 255).ToString();
    //    Hexadecimal = color.ToHex();
    //}
    //partial void OnGreenChanged(string? value)
    //{
    //    if (!Color.TryParse(value, out Color color)) return;
    //    Green = Math.Round(color.Green * 255).ToString();
    //}
    //partial void OnBlueChanged(string? value)
    //{
    //    if (!Color.TryParse(value, out Color color)) return;
    //    Blue = Math.Round(color.Blue * 255).ToString();
    //}
    //partial void OnAlphaChanged(string? value)
    //{
    //    if (!Color.TryParse(value, out Color color)) return;
    //    Alpha = Math.Round(color.Alpha * 255).ToString();
    //}
    //partial void OnHexadecimalChanged(string? value)
    //{
    //    if (!string.IsNullOrEmpty(value) && value.Length >= 6)
    //    {
    //        string argbHex = value;
    //        if (value.Length == 8)
    //        {
    //            string a = value.Substring(6, 2);
    //            string rgb = value[..6];
    //            argbHex = a + rgb;
    //        }

    //        if (Color.TryParse("#" + argbHex, out Color color))
    //        {
    //            UpdateColorComponents(color);
    //            CurrentColor = color;
    //        }
    //    }
    //}

    //async Task UpdateColorFromComponents()
    //{
    //    if (!string.IsNullOrEmpty(Red) && !string.IsNullOrEmpty(Green) &&
    //        !string.IsNullOrEmpty(Blue) && !string.IsNullOrEmpty(Alpha))
    //    {
    //        var vRed = float.Parse(Red) / 255;
    //        var vGreen = float.Parse(Green) / 255;
    //        var vBlue = float.Parse(Blue) / 255;
    //        var vAlpha = float.Parse(Alpha) / 255;
    //        CurrentColor = Color.FromRgba(vRed, vGreen, vBlue, vAlpha);
    //        Hexadecimal = CurrentColor.ToHex();
    //    }
    //    await Task.CompletedTask;
    //}

    #region EXTRA

    #endregion
}
