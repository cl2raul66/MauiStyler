using MauiStylerApp.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiStylerApp.Views;

public partial class PgStyleEditor : ContentPage
{
    public PgStyleEditor(PgStyleEditorViewModel vm)
    {
        InitializeComponent();

        vm.InitializerPropertyAsync();
        BindingContext = vm;
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = sender as CollectionView;

        if (collectionView is not null)
        {
            if (e.CurrentSelection.Count == 0)
            {
                collectionView.SelectionMode = SelectionMode.None;
            }
            collectionView.SelectionMode = SelectionMode.Single;
        }
    }

    #region EXTRA

    #endregion
}