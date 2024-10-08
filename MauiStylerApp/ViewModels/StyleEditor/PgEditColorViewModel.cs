﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiStylerApp.Models;
using MauiStylerApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MauiStylerApp.ViewModels;

[QueryProperty(nameof(SendToken), nameof(SendToken))]
[QueryProperty(nameof(CurrentColorStyle), nameof(CurrentColorStyle))]
public partial class PgEditColorViewModel : ObservableValidator
{
    readonly IColorsPalettesService colorsPalettesServ;

    public PgEditColorViewModel(IColorsPalettesService colorsPalettesService)
    {
        colorsPalettesServ = colorsPalettesService;
        Palettes = [.. colorsPalettesServ.GetAll()];
        if (Palettes.Count > 0)
        {
            SelectedPaletteItem = Palettes[0];
        }
    }

    [ObservableProperty]
    string? sendToken;

    [ObservableProperty]
    ColorStyle? currentColorStyle;

    [ObservableProperty]
    ObservableCollection<ColorPalette>? palettes;

    [ObservableProperty]
    ColorPalette? selectedPaletteItem;

    [ObservableProperty]
    ObservableCollection<Color>? colorsOfPalette;

    [ObservableProperty]
    Color? selectedColorOfPalette;

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

        ColorStyle colorStyle = new()
        {
            Name = NameColor!.Trim().ToUpper(),
            Value = CurrentColor
        };

        //string tokenSend = CurrentColor is null ? "NewItemColor" : "EditItemColor";

        _ = WeakReferenceMessenger.Default.Send(colorStyle, SendToken!);

        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    async Task GoToBack()
    {
        _ = WeakReferenceMessenger.Default.Send("cancel", "F4E5D6C7-B8A9-0B1C-D2E3-F4567890ABCD");
        await Shell.Current.GoToAsync("..", true);
    }

    partial void OnIsDefaultColorChanged(bool value)
    {
        if (value && CurrentColorStyle is not null)
        {
            LastColorSelected = CurrentColorStyle.Value;
        }
    }

    partial void OnCurrentColorStyleChanged(ColorStyle? value)
    {
        IsDefaultColor = value is not null;
        if (value is not null)
        {
            NameColor = value.Name;
        }
    }

    partial void OnSelectedPaletteItemChanged(ColorPalette? value)
    {
        if (value is null) return;

        ColorsOfPalette = [.. value.ColorsList!.Values];        
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

    #region EXTRA

    #endregion
}
