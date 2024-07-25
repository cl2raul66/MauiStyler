using MauiStyler.App.ViewModels;

namespace MauiStyler.App.Views;

public partial class PgStyleEditor : ContentPage
{
	public PgStyleEditor(PgStyleEditorViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}