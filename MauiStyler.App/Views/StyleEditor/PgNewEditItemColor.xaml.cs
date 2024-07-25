using MauiStyler.App.ViewModels;

namespace MauiStyler.App.Views;

public partial class PgNewEditItemColor : ContentPage
{
	public PgNewEditItemColor(PgNewEditItemColorViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}