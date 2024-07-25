using MauiStyler.App.Views;

namespace MauiStyler.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(PgStyleEditor), typeof(PgStyleEditor));
        Routing.RegisterRoute(nameof(PgNewEditItemColor), typeof(PgNewEditItemColor));
    }
}
