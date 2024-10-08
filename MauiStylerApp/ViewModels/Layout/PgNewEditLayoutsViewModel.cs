﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiStylerApp.Models;
using MauiStylerApp.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiStylerApp.ViewModels;

public partial class PgNewEditLayoutsViewModel : ObservableValidator
{
    public PgNewEditLayoutsViewModel()
    {
        InitializeGvDrawables();
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    #region VISUALIZACIÓN
    public float WidthGraphicsView => IsLandscapeView ? 1024 : 375;
    public float HeightGraphicsView => IsLandscapeView ? 600 : 667;

    [ObservableProperty]
    ObservableCollection<GdItem>? gvDrawables;

    [ObservableProperty]
    GdItem? selectedGvDrawable;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WidthGraphicsView))]
    [NotifyPropertyChangedFor(nameof(HeightGraphicsView))]
    bool isLandscapeView;

    [RelayCommand]
    void SetPortraitView()
    {
        IsLandscapeView = false;
    }

    [RelayCommand]
    void SetLandscapeView()
    {
        IsLandscapeView = true;
    }

    [RelayCommand]
    void AddControl()
    {
        var newRect = new GdItem
        {
            Bounds = new RectF(0.25f, 0.25f, 0.5f, 0.5f),
            BorderColor = GetRandomColor(),
            FillColor = GetRandomColor()
        };

        if (SelectedGvDrawable is not null)
        {
            SelectedGvDrawable.Children.Add(newRect);
            newRect.Parent = SelectedGvDrawable;
        }
        else
        {
            GvDrawables!.Add(newRect);
        }
    }

    Color GetRandomColor()
    {
        Random rand = new();
        int r = rand.Next(256);
        int g = rand.Next(256);
        int b = rand.Next(256);
        return Color.FromRgba(r, g, b, 255);
    }

    void InitializeGvDrawables()
    {
        var first = new GdItem
        {
            Bounds = new RectF(0, 0, 1, 1),
            BorderColor = Colors.Black,
            FillColor = Colors.White
        };
        GvDrawables = [first];
    }
    #endregion
}
