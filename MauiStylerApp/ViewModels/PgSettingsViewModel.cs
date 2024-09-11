using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStylerApp.Models;
using MauiStylerApp.Services;
using MauiStylerApp.Tools;
using System.Collections.ObjectModel;

namespace MauiStylerApp.ViewModels;

public partial class PgSettingsViewModel : ObservableObject
{
    readonly IColorsPalettesService colorsPalettesServ;

    public PgSettingsViewModel(IColorsPalettesService colorsPalettesService)
    {
        colorsPalettesServ = colorsPalettesService;
    }

    #region Paleta de colores
    [ObservableProperty]
    ObservableCollection<ColorPaletteItem>? paletteItems;

    [ObservableProperty]
    ColorPaletteItem? selectedaPaletteItem;

    [RelayCommand]
    async Task ImportGIMPPaletteFile()
    {
        var files = await FileHelper.OpenFileGPL();
        if (files.Any())
        {
            ColorPalette[] newPalette = GimpPaletteHelper.ReadGimpPalette([.. files.Select(x => x.FullPath)]);
            PaletteItems ??= [];
            var result = colorsPalettesServ.InsertMany(newPalette);
            if (result)
            {
                foreach (var item in newPalette)
                {
                    PaletteItems.Add(new() { Name = item.Name, Length = item.ColorsList?.Count ?? 0 });
                }
            }
        }
        SelectedaPaletteItem = null;
    }

    [RelayCommand]
    void DeletedPalette()
    {
        int idx = PaletteItems!.IndexOf(SelectedaPaletteItem!);
        PaletteItems.RemoveAt(idx);
    }
    #endregion

    [RelayCommand]
    async Task GoToBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    #region EXTRA
    public async void Initialize()
    {
        PaletteItems ??= [];
        PaletteItems = [.. colorsPalettesServ.GetAll().Select(x => new ColorPaletteItem() { Name = x.Name, Length = x.ColorsList?.Count ?? 0 })];
        await Task.CompletedTask;
    }
    #endregion
}
