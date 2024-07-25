using CommunityToolkit.Mvvm.ComponentModel;
using MauiStyler.App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiStyler.App.ViewModels;

[QueryProperty(nameof(NameCurrent), nameof(NameCurrent))]
[QueryProperty(nameof(CurrentItemColor), nameof(CurrentItemColor))]
public partial class PgNewEditItemColorViewModel : ObservableValidator
{
    [ObservableProperty]
    string? nameCurrent;

    [ObservableProperty]
    ItemColor? currentItemColor;

    public string Title => CurrentItemColor is null ? "Nuevo" : "Modificar";

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(NameCurrent))
        {

        }

        if (e.PropertyName == nameof(CurrentItemColor))
        {

        }
    }
}
