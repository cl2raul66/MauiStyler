using MauiStyler.App.ViewModels;

namespace MauiStyler.App.Views;

public partial class PgMain : ContentPage
{
    public PgMain(PgMainViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}