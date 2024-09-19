using MauiStylerApp.ViewModels;

namespace MauiStylerApp.Views;

public partial class PgLayouts : ContentPage
{
	public PgLayouts(PgLayoutsViewModel vm)
	{
		InitializeComponent();

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
}