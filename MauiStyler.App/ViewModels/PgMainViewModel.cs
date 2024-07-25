using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStyler.App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiStyler.App.ViewModels;

public partial class PgMainViewModel : ObservableObject
{

    [RelayCommand]
    async Task GoToPgStyleEditor()
    {
        Dictionary<string, object> sendData = new()
        {
            {"title", "Nuevo"}
        };
        await Shell.Current.GoToAsync(nameof(PgStyleEditor), true, sendData);
    }
}
