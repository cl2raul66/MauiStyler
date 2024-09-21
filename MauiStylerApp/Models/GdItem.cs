using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MauiStylerApp.Models;

public partial class GdItem : ObservableObject
{
    [ObservableProperty]
    RectF bounds;

    [ObservableProperty]
    Color? borderColor;

    [ObservableProperty]
    Color? fillColor;

    [ObservableProperty]
    bool isSelected;

    [ObservableProperty]
    ObservableCollection<GdItem>children = [];

    [ObservableProperty]
    GdItem? parent;
}
