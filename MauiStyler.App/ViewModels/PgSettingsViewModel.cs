using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStyler.App.Models;
using MauiStyler.App.Tools;
using System.Collections.ObjectModel;

namespace MauiStyler.App.ViewModels;

public partial class PgSettingsViewModel : ObservableObject
{
    #region Paleta de colores
    [ObservableProperty]
    ObservableCollection<ColorPaletteItem>? paletteItems;

    [ObservableProperty]
    ColorPaletteItem? selectedaPaletteItem;

    [RelayCommand]
    async Task ImportGIMPPaletteFile()
    {
        var files = await FileHelper.OpenFileGPL();
        ColorPalette[] newPalette = GimpPaletteHelper.ReadGimpPalette([.. files.Select(x => x.FullPath)]);
        PaletteItems ??= [];
        foreach (var item in newPalette)
        {
            PaletteItems.Add(new() { Name = item.Name, Length = item.ColorsList?.Length ?? 0 });
        }
    }

    #endregion

    [RelayCommand]
    async Task GoToBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }
}
