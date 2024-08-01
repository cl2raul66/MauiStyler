using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStyler.App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiStyler.App.ViewModels;

[QueryProperty(nameof(NameCurrent), nameof(NameCurrent))]
[QueryProperty(nameof(CurrentItemColor), nameof(CurrentItemColor))]
public partial class PgNewEditItemColorViewModel : ObservableValidator
{
    public PgNewEditItemColorViewModel()
    {
        GetColorPaletteMAUI();
    }

    [ObservableProperty]
    string? nameCurrent;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    ItemColor? currentItemColor;

    [ObservableProperty]
    List<Color>? colorPalette;

    [ObservableProperty]
    Color? selectedColorOfPalette;

    public string Title => CurrentItemColor is null ? "Nuevo" : "Modificar";

    [ObservableProperty]
    string? nameColor;

    [RelayCommand]
    async Task GoToBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(NameCurrent))
        {

        }

        if (e.PropertyName == nameof(CurrentItemColor))
        {
            if (CurrentItemColor is not null)
            {
                NameColor = CurrentItemColor.Name;
            }
        }
    }

    #region EXTRA
    void GetColorPaletteMAUI()
    {
        var allColors = new List<Color>();
        var colorType = typeof(Colors);

        foreach (var field in colorType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (field.FieldType == typeof(Color))
            {
                var color = (Color)field.GetValue(null)!;
                allColors.Add(color);
            }
        }

        ColorPalette = allColors;
    }
    #endregion
}
