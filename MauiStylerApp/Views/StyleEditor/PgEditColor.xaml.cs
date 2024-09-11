using MauiStylerApp.ViewModels;

namespace MauiStylerApp.Views;

public partial class PgEditColor : ContentPage
{
    public PgEditColor(PgEditColorViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}

    void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
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