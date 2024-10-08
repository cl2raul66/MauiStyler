﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStylerApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiStylerApp.ViewModels;

public partial class PgLayoutsViewModel : ObservableRecipient
{
    [RelayCommand]
    async Task GoToMain()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    async Task GoToNewLayout()
    {
        await Shell.Current.GoToAsync(nameof(PgNewEditLayouts), true);
    }
}
