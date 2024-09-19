using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
}
