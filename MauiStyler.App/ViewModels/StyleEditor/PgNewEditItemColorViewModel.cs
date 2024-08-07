using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStyler.App.Models;
using System.ComponentModel;
using System.Reflection;

namespace MauiStyler.App.ViewModels;

[QueryProperty(nameof(CurrentSeptionName), nameof(CurrentSeptionName))]
[QueryProperty(nameof(CurrentItemColor), nameof(CurrentItemColor))]
public partial class PgNewEditItemColorViewModel : ObservableValidator
{
    public PgNewEditItemColorViewModel()
    {
        GetColorPaletteMAUI();
    }

    [ObservableProperty]
    string? currentSeptionName;

    [ObservableProperty]
    ItemColor? currentItemColor;

    [ObservableProperty]
    List<Color>? colorPalette;

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
    string? title = "Nuevo";

    [ObservableProperty]
    string? nameColor;

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

            SelectedColorOfPalette = null;
        }
    }

    [RelayCommand]
    async Task Save()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    async Task GoToBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(CurrentItemColor))
        {
            if (CurrentItemColor is not null)
            {
                NameColor = CurrentItemColor.Name;
                Title = "Modificar";

                if (CurrentItemColor.Value is null)
                {
                    return;
                }

                DefaultColor = CurrentItemColor.Value;
                IsDefaultColor = true;
            }
        }

        if (e.PropertyName == nameof(IsDefaultColor))
        {
            LastColorSelected = IsDefaultColor ? DefaultColor : SelectedColorOfPalette;
        }

        if (e.PropertyName == nameof(SelectedColorOfPalette))
        {
            if (SelectedColorOfPalette is not null)
            {
                Red = (SelectedColorOfPalette.Red * 255).ToString();
                Green = (SelectedColorOfPalette.Green * 255).ToString();
                Blue = (SelectedColorOfPalette.Blue * 255).ToString();
                Alpha = (SelectedColorOfPalette.Alpha * 255).ToString();
                Hexadecimal = SelectedColorOfPalette.ToRgbaHex(true)[1..];
            }
        }

        if (e.PropertyName == nameof(CurrentColor))
        {
            
        }

        if (e.PropertyName == nameof(Red))
        {
            if (!string.IsNullOrEmpty(Red) && !string.IsNullOrEmpty(Green) && !string.IsNullOrEmpty(Blue) && !string.IsNullOrEmpty(Alpha))
            {
                var vRed = float.Parse(Red!) / 255;
                var vGreen = float.Parse(Green!) / 255;
                var vBlue = float.Parse(Blue!) / 255;
                var vAlpha = float.Parse(Alpha!) / 255;
                CurrentColor = Color.FromRgba(vRed, vGreen, vBlue, vAlpha);
                Hexadecimal = CurrentColor.ToRgbaHex(true)[1..];
            }
        }

        if (e.PropertyName == nameof(Green))
        {
            if (!string.IsNullOrEmpty(Red) && !string.IsNullOrEmpty(Green) && !string.IsNullOrEmpty(Blue) && !string.IsNullOrEmpty(Alpha))
            {
                var vRed = float.Parse(Red!) / 255;
                var vGreen = float.Parse(Green!) / 255;
                var vBlue = float.Parse(Blue!) / 255;
                var vAlpha = float.Parse(Alpha!) / 255;
                CurrentColor = Color.FromRgba(vRed, vGreen, vBlue, vAlpha);
                Hexadecimal = CurrentColor.ToRgbaHex(true)[1..];
            }
        }

        if (e.PropertyName == nameof(Blue))
        {
            if (!string.IsNullOrEmpty(Red) && !string.IsNullOrEmpty(Green) && !string.IsNullOrEmpty(Blue) && !string.IsNullOrEmpty(Alpha))
            {
                var vRed = float.Parse(Red!) / 255;
                var vGreen = float.Parse(Green!) / 255;
                var vBlue = float.Parse(Blue!) / 255;
                var vAlpha = float.Parse(Alpha!) / 255;
                CurrentColor = Color.FromRgba(vRed, vGreen, vBlue, vAlpha);
                Hexadecimal = CurrentColor.ToRgbaHex(true)[1..];
            }
        }

        if (e.PropertyName == nameof(Alpha))
        {
            if (!string.IsNullOrEmpty(Red) && !string.IsNullOrEmpty(Green) && !string.IsNullOrEmpty(Blue) && !string.IsNullOrEmpty(Alpha))
            {
                var vRed = float.Parse(Red!) / 255;
                var vGreen = float.Parse(Green!) / 255;
                var vBlue = float.Parse(Blue!) / 255;
                var vAlpha = float.Parse(Alpha!) / 255;
                CurrentColor = Color.FromRgba(vRed, vGreen, vBlue, vAlpha);
                Hexadecimal = CurrentColor.ToRgbaHex(true)[1..];
            }
        }
    }

    protected override void OnPropertyChanging(System.ComponentModel.PropertyChangingEventArgs e)
    {
        base.OnPropertyChanging(e);

        if (e.PropertyName == nameof(SelectedColorOfPalette))
        {
            if (SelectedColorOfPalette is not null && !IsDefaultColor)
            {
                LastColorSelected = SelectedColorOfPalette;
            }
        }
    }

    #region EXTRA
    public void GetColorPaletteMAUI()
    {
        HashSet<Color> allColors = [];
        var colorType = typeof(Colors);

        foreach (var field in colorType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (field.FieldType == typeof(Color))
            {
                var color = (Color)field.GetValue(null)!;
                allColors.Add(color);
            }
        }

        if (allColors.Count != 0)
        {
            ColorPalette = [.. allColors];
            if (ColorPalette is not null && ColorPalette.Count > 0)
            {
                SelectedColorOfPalette = ColorPalette[0];
            }
        }
        else
        {
            ColorPalette = [Colors.White];
            SelectedColorOfPalette = ColorPalette[0];
        }
    }

    #endregion
}
