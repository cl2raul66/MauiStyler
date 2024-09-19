using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStylerApp.Views;

namespace MauiStylerApp.ViewModels;

public partial class PgMainViewModel : ObservableObject
{
    [RelayCommand]
    async Task GoToSetting()
    {
        await Shell.Current.GoToAsync(nameof(PgSettings), true);
    }

    [RelayCommand]
    async Task GoToThemes()
    {
        await Shell.Current.GoToAsync(nameof(PgThemes), true);
    }

    [RelayCommand]
    async Task GoToNewThemes()
    {
        await Shell.Current.GoToAsync($"{nameof(PgThemes)}/{nameof(PgStyleEditor)}", true);
    }
    #region EXTRA
    
    #endregion
}
