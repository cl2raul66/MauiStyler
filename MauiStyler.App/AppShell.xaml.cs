using MauiStyler.App.Views;

namespace MauiStyler.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(PgStyleEditor), typeof(PgStyleEditor));
    }
}
