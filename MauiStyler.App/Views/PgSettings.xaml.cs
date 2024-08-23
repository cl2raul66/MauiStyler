using MauiStyler.App.ViewModels;

namespace MauiStyler.App.Views;

public partial class PgSettings : ContentPage
{
	public PgSettings(PgSettingsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}