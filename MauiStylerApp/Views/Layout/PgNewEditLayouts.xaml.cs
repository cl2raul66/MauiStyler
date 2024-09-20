using MauiStylerApp.ViewModels;

namespace MauiStylerApp.Views;

public partial class PgNewEditLayouts : ContentPage
{
	public PgNewEditLayouts(PgNewEditLayoutsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}