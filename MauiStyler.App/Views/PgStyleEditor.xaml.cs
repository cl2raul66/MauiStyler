using MauiStyler.App.ViewModels;

namespace MauiStyler.App.Views;

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
            //if (collectionView == CvPrincipals)
            //{
            //    if (e.CurrentSelection.Count > 0)
            //    {
            //        collectionView.SelectionMode = SelectionMode.None;
            //    }
            //    collectionView.SelectionMode = SelectionMode.Single;

            //    return;
            //}
            //if (collectionView == CvPrincipalsDark)
            //{
            //    if (e.CurrentSelection.Count > 0)
            //    {
            //        collectionView.SelectionMode = SelectionMode.None;
            //    }
            //    collectionView.SelectionMode = SelectionMode.Single;

            //    return;
            //}
            //if (collectionView == CvSemantics)
            //{
            //    if (e.CurrentSelection.Count > 0)
            //    {
            //        collectionView.SelectionMode = SelectionMode.None;
            //    }
            //    collectionView.SelectionMode = SelectionMode.Single;

            //    return;
            //}
            //if (collectionView == CvSemanticsDark)
            //{
            //    if (e.CurrentSelection.Count > 0)
            //    {
            //        collectionView.SelectionMode = SelectionMode.None;
            //    }
            //    collectionView.SelectionMode = SelectionMode.Single;

            //    return;
            //}
            //if (collectionView == CvNeutrals)
            //{
            //    if (e.CurrentSelection.Count > 0)
            //    {
            //        collectionView.SelectionMode = SelectionMode.None;
            //    }
            //    collectionView.SelectionMode = SelectionMode.Single;

            //    return;
            //}
            //if (collectionView == CvNeutralsDark)
            //{
            //    if (e.CurrentSelection.Count > 0)
            //    {
            //        collectionView.SelectionMode = SelectionMode.None;
            //    }
            //    collectionView.SelectionMode = SelectionMode.Single;

            //    return;
            //}

            if (e.CurrentSelection.Count == 0)
            {
                collectionView.SelectionMode = SelectionMode.None;
            }
            collectionView.SelectionMode = SelectionMode.Single;
        }
    }
}